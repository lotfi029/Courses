using Courses.Business.Contract.Exam;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
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
        exam.IsDisable = true;

        await _context.Exams.AddAsync(exam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(exam.Id);
    }
    public async Task<Result> AddExamQuestionsAsync(int id, Guid moduleId, string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default)
    {

        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == id, cancellationToken) is not { } exam)
            return ExamErrors.NotFoundExam;

        var courseId = await _context.Modules
            .Where(e => e.Id == moduleId && e.CreatedById == userId)
            .Select(e => e.CourseId)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (Guid.Empty == courseId)
            return ModuleErrors.NotFound;


        var questions = await _context.Questions
            .Where(e => e.CourseId == courseId)
            .Select(e => new {e.IsDisable, e.Id})
            .ToListAsync(cancellationToken);

        var examQuestionValidate = await _context.ExamQuestion
            .Where(e => e.ExamId == id)
            .ToListAsync(cancellationToken);

        
        List<ExamQuestion> examQuestions = [];
        foreach (var questionId in questionIds)
        {
            if (!questions.Contains(new { IsDisable = false, Id = questionId }))
                return QuestionErrors.NotFoundQuestion;
            
            var examQuestion = new ExamQuestion { ExamId = id, QuestionId = questionId };
            
            if (!examQuestionValidate.Contains(examQuestion))
                examQuestions.Add(examQuestion);
        }
        exam.NoQuestion += examQuestions.Count;
        
        await _context.ExamQuestion.AddRangeAsync(examQuestions, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
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
            select new { e.Id, e.Title, e.Description, e.Duration, user }
            ).FirstOrDefaultAsync(cancellationToken);

        if (exam is null)
            return Result.Failure<ExamResponse>(ExamErrors.NotFoundExam);

        List<QuestionResponse> question;
        if (exam.user is not null)
        {
            question = await (
                from q in _context.Questions
                join ua in _context.Answers.Where(e => exam.user != null && exam.user.Id == e.Id)
                on q.Id equals ua.QuestionId into answers
                from uAnswers in answers.DefaultIfEmpty()
                select new QuestionResponse(q.Id, q.Text, q.IsDisable, null, uAnswers == null ? null : uAnswers.OptionId)
                ).ToListAsync(cancellationToken);
        }
        else
        {
            question = await _context.Questions
                .Where(e => !e.IsDisable)
                .AsNoTracking()
                .ProjectToType<QuestionResponse>()
                .ToListAsync(cancellationToken);
        }
        var response = new ExamResponse(exam.Id, exam.Title, exam.Description, exam.Duration, question, exam.user?.Score);

        return Result.Success(response);
    }
}
