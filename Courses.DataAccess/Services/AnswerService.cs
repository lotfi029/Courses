using Courses.Business.Contract.Answer;
using Courses.Business.Contract.Exam;
using Courses.Business.Contract.Question;
using Courses.Business.Contract.UserExam;
using Courses.Business.Entities;
using Serilog.Configuration;

namespace Courses.DataAccess.Services;
public class AnswerService(ApplicationDbContext context) : IAnswerService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAnswer(Guid examId, string userId, IEnumerable<AnswerValues> request, CancellationToken cancellationToken = default)
    {
        if (await _context.UserExams.SingleOrDefaultAsync(e => e.UserId == userId && e.ModuleItemId == examId && e.EndDate == null,cancellationToken) is not { } userExam)
            return UserExamErrors.InvalidSubmitExam; // exam not found

        var examQuestionid = await _context.ExamQuestion
            .Where(e => e.ExamId == examId)
            .AsNoTracking()
            .Select(e => e.QuestionId)
            .ToListAsync(cancellationToken);

        if (examQuestionid.Count == 0 || request.Count() != examQuestionid.Count)
            return QuestionErrors.NotFoundQuestion; // wrong in selected question
        
        float degree = 100 / examQuestionid.Count;

        var options = await _context.Options
            .Where(e => examQuestionid.Contains(e.QuestionId) && e.IsCorrect)
            .Select(e => new AnswerValues (e.QuestionId, e.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (options.Count != examQuestionid.Count)
            return QuestionErrors.NotFoundOptions; // wrong in options

        List<Answer> answers = [];
        float score = 0;
        foreach(var question in request)
        {
            if (options.Contains(question))
                score += degree;

            answers.Add(new()
            {
                QuestionId = question.QuestionId,
                UserExamId = userExam.Id,
                OptionId = question.OptionId,
            });
        }

        userExam.Score = score;
        userExam.EndDate = DateTime.UtcNow;

        userExam.Duration = userExam.StartDate - userExam.EndDate;

        await _context.Answers.AddRangeAsync(answers, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result<ExamResponse>> EnrollExamAsync(Guid examId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == examId, cancellationToken) is not { } exam)
            return Result.Failure<ExamResponse>(ExamErrors.NotFoundExam); // see error class TODO:

        if (exam.CreatedById == userId)
            return Result.Failure<ExamResponse>(ExamErrors.InvalidEnrollment);

        if (await _context.UserExams.AnyAsync(e => e.ModuleItemId == examId && e.UserId == userId, cancellationToken))
            return Result.Failure<ExamResponse>(UserExamErrors.DuplicatedAnswer);

        if (exam.IsDisable)
            return Result.Failure<ExamResponse>(ExamErrors.ExamedNotAvailable);

        var userExam = new UserExam
        {
            ModuleItemId = examId,
            UserId = userId
        };

        await _context.UserExams.AddAsync(userExam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await GetEnroledExam(exam, cancellationToken);

        return Result.Success(response);
    }
    public async Task<Result<ExamResponse>> ReEnrolExamAsync(Guid examId, string userId, CancellationToken cancellationToken = default)
    {
        var userExams = await _context.UserExams.Where(e => e.ModuleItemId == examId && e.UserId == userId).ToListAsync(cancellationToken);
        
        if (userExams.Count == 0)
            return Result.Failure<ExamResponse>(UserExamErrors.InvalidEnrollment);
       
        if (userExams.Any(e => e.Score >= 70))
            return Result.Failure<ExamResponse>(UserExamErrors.DuplicatedAnswer);

        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == examId && !e.IsDisable, cancellationToken) is not { } exam)
            return Result.Failure<ExamResponse>(UserExamErrors.ExamNotAvailable);

        var userExam = new UserExam                                                        
        {
            ModuleItemId = examId,
            UserId = userId
        };

        await _context.UserExams.AddAsync(userExam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await GetEnroledExam(exam, cancellationToken);

        return Result.Success(response); 
    }
    public async Task<Result<UserExamDetailResponse>> GetAsync(Guid examId, string userId, CancellationToken cancellationToken)
    {
        var exam = await _context.Exams
            .SingleOrDefaultAsync(e => e.Id == examId, cancellationToken);

        if (exam is null)
            return Result.Failure<UserExamDetailResponse>(ExamErrors.NotFoundExam);

        var response = await GetUserExams([exam], userId, cancellationToken);

        return Result.Success(response.FirstOrDefault())!;
    }
    public async Task<IEnumerable<UserExamDetailResponse>> GetAllAsync(Guid moduleId, string userId, CancellationToken cancellationToken = default)
    {
        
        var courseId = await _context.Modules
            .Where(e => e.Id == moduleId)
            .AsNoTracking()
            .Select(e => e.CourseId)
            .SingleOrDefaultAsync(cancellationToken);

        var moduleIds = await _context.Modules
            .Where(e => e.CourseId == courseId)
            .AsNoTracking()
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);
        
        var exams = await _context.Exams
            .Where(e => moduleIds.Contains(e.ModuleId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        var response = await GetUserExams(exams, userId, cancellationToken);

        return response;
    }
    public async Task<IEnumerable<UserExamResponse>> GetExamUsersAsync(Exam exam, CancellationToken cancellationToken = default)
    {
        var userExams = await _context.UserExams
           .Where(e => e.ModuleItemId == exam.Id)
           .AsNoTracking()
           .ProjectToType<UserExamResponse>()
           .ToListAsync(cancellationToken);

        return userExams;
    }
    private async Task<IEnumerable<UserExamDetailResponse>> GetUserExams(List<Exam> exams, string userId, CancellationToken cancellationToken)
    {
        var query = await (
            from ue in _context.UserExams.Where(e => e.UserId == userId)
            join eq in _context.ExamQuestion
            on ue.ModuleItemId equals eq.ExamId
            join q in _context.Questions
            on eq.QuestionId equals q.Id
            join a in _context.Answers
            on new { UserExamId = ue.Id, eq.QuestionId } equals new { a.UserExamId, a.QuestionId }
            select new
            {
                ue.Id,
                ue.ModuleItemId,
                ue.Duration,
                ue.StartDate,
                ue.EndDate,
                ue.Score,
                ue.UserId,
                questions = new UserQuestionResponse(
                    q.Id,
                    q.Text,
                    a.OptionId == q.Options.Where(e => e.IsCorrect).First().Id,
                    a.OptionId,
                    q.Options.Adapt<List<UserOptionResponse>>()
                )
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var userQuestions = query
            .GroupBy(g => new { g.Id, g.ModuleItemId, g.Duration, g.StartDate, g.EndDate, g.Score, g.UserId })
            .Select(x => new {
                x.Key.ModuleItemId,
                userExams = new UserExamResponse(
                x.Key.Id,
                x.Key.Duration,
                x.Key.StartDate,
                x.Key.EndDate,
                x.Key.Score,
                x.Key.UserId,
                x.Select(e => e.questions).ToList()
                )
            });

        var response = from e in exams
                       join uq in userQuestions
                       on e.Id equals uq.ModuleItemId into userExam
                       select new UserExamDetailResponse(
                           e.Id,
                           e.Title,
                           e.Description,
                           e.Duration,
                           e.NoQuestion,
                           userQuestions.Count(),
                           userExam.Select(u => u.userExams)
                       );

        return response;
    }

    private async Task<ExamResponse> GetEnroledExam(Exam exam, CancellationToken cancellationToken)
    {
        
        var questions = await (
            from eq in _context.ExamQuestion
            join q in _context.Questions
            on eq.QuestionId equals q.Id
            where eq.ExamId == exam.Id
            select new QuestionResponse(
                q.Id, 
                q.Text,
                q.IsDisable,
                q.Options.Adapt<List<OptionResponse>>()
                )
            ).ToListAsync(cancellationToken);

        ExamResponse response = new(exam.Id, exam.Title, exam.Description, exam.Duration,null, questions);

        return response;
    }
}
