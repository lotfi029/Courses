using Courses.Business.Contract.Exam;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection;

namespace Courses.DataAccess.Services;
public class ExamService(ApplicationDbContext context) : IExamService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<int>> AddAsync(Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Modules.AnyAsync(e => e.Id == moduleId && e.CreatedById == userId, cancellationToken))
            return Result.Failure<int>(ModuleErrors.NotFound);

        if (await _context.Exams.AnyAsync(e => e.Title == request.Title && e.ModuleId == moduleId, cancellationToken))
            return Result.Failure<int>(ExamErrors.DuplicatedTitle);

        var exam = request.Adapt<Exam>();
        exam.ModuleId = moduleId;

        await _context.Exams.AddAsync(exam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(exam.Id);
    }

    public async Task<Result> UpdateAsync(int id, Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.SingleAsync(e => e.Id == id && e.CreatedById == userId, cancellationToken) is not { } exam)
            return ExamErrors.NotFoundExam;

        if (!await _context.Exams.AnyAsync(e => e.Title == request.Title && e.ModuleId == moduleId, cancellationToken))
            return ModuleErrors.DuplicatedTitle;

        exam = request.Adapt(exam);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleAsync(int id, string userId, CancellationToken cancellationToken = default)
    {
        int rowUpdated = await _context.Exams
            .Where(e => e.Id == id && e.CreatedById == userId)
            .ExecuteUpdateAsync(setters => 
                setters
                .SetProperty(e => e.IsDisable, e => !e.IsDisable),
                cancellationToken
            );

        return rowUpdated == 0 ? ExamErrors.NotFoundExam : Result.Success();
    }
    public async Task<Result<ExamResponse>> GetExamAsync(int id, string userId, CancellationToken cancellationToken = default)
    {

        //float? score;
        //int? userExamId;
        var exam = await (
            from e in _context.Exams
            join ue in _context.UserExams.Where(x => userId == x.UserId)
            on e.Id equals ue.ExamId into ue
            from user in ue.DefaultIfEmpty()
            where e.Id == id
            select new { e.Id, e.Title, e.Description, e.Degree, e.Duration, user}
            ).FirstOrDefaultAsync(cancellationToken);

        if (exam is null)
            return Result.Failure<ExamResponse>(ExamErrors.NotFoundExam);

        List<QuestionResponse> question;
        if (exam.user is not null)
        {
            question = await (
                from q in _context.Questions
                join ua in _context.UserAnswers.Where(e => exam.user != null && exam.user.Id == e.Id)
                on q.Id equals ua.QuestionId into answers
                from uAnswers in answers.DefaultIfEmpty()
                where q.ExamId == id
                select new QuestionResponse(q.Id, q.Text, uAnswers.OptionId == q.Options.Where(e => e.IsCorrect).Select(e => id).First() ? q.Points : 0, q.Options.Adapt<List<OptionResponse>>(), uAnswers.OptionId)
                ).ToListAsync(cancellationToken);
        }
        else
        {
            question = await _context.Questions
                .Where(e => e.ExamId == id)
                .AsNoTracking()
                .ProjectToType<QuestionResponse>()
                .ToListAsync(cancellationToken);
        }
        var response = new ExamResponse(exam.Id, exam.Title, exam.Description, exam.Degree, exam.Duration, question, exam.user?.Score);

        return Result.Success(response);
    }

    public async Task<Result> AddQuestionsAsync(int examId, string userId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.AnyAsync(e => e.Id == examId && e.CreatedById != userId, cancellationToken))
            return ExamErrors.NotFoundExam;

        var question = request.Adapt<Question>();
        question.ExamId = examId;

        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateQuestionsAsync(int id, int examId, string userId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.AnyAsync(e => e.Id == examId && e.CreatedById == userId, cancellationToken))
            return ExamErrors.NotFoundExam;

        var rowsUpdated = await _context.Questions
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Text, request.Text)
                .SetProperty(e => e.Points, request.Points),
                cancellationToken
            );

        return rowsUpdated == 0 ? ExamErrors.NotFoundQuestion : Result.Success();
    }
    public async Task<Result> RemoveQuestionAsync(int id, int examId, string userId, CancellationToken cancellationToken = default)
    {
        if (!await _context.Exams.AnyAsync(e => e.Id == examId && e.CreatedById == userId, cancellationToken))
            return ExamErrors.NotFoundExam;

        var rowsDeleted = await _context.Questions
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted == 0 ? ExamErrors.NotFoundQuestion : Result.Success();
    }
    public async Task<Result> AddOptionAsync(int questionId, int examId, string userId, IEnumerable<OptionRequest> request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Exams.AnyAsync(e => e.Id == examId && e.CreatedById == userId, cancellationToken))
            return ExamErrors.NotFoundExam;

        if (!await _context.Questions.AnyAsync(e => e.Id == questionId, cancellationToken))
            return ExamErrors.NotFoundQuestion;

        var existOptions = await _context.Optinos
            .Where(e => e.QuestionId == questionId)
            .ToListAsync(cancellationToken);

        List<Option> options = [];
        foreach (var option in request)
        {
            if (existOptions.Select(e => e.Text).Contains(option.Text))
                return ExamErrors.DuplicatedOptionsText;

            options.Add(new()
            {
                IsCorrect = option.IsCorrect,
                QuestionId = questionId,
                Text = option.Text,
            });
        }

        await _context.Optinos.AddRangeAsync(options, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateOptionAsync(int id, int examId, string userId, OptionRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.AnyAsync(e => e.Id == examId && e.CreatedById != userId, cancellationToken))
            return ExamErrors.NotFoundExam;

        var rowsUpdated = await _context.Optinos
           .Where(e => e.Id == id)
           .ExecuteUpdateAsync(setters => setters
               .SetProperty(e => e.Text, request.Text)
               .SetProperty(e => e.IsCorrect, request.IsCorrect),
               cancellationToken
           );

        return rowsUpdated == 0 ? ExamErrors.NotFoundOptions : Result.Success();
    }
    public async Task<Result> RemoveOptionAsync(int id, int examId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.AnyAsync(e => e.Id == examId && e.CreatedById != userId, cancellationToken))
            return ExamErrors.NotFoundExam;

        var rowsDeleted = await _context.Optinos
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted == 0 ? ExamErrors.NotFoundOptions : Result.Success();
    }
}
