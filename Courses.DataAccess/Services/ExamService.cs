using Courses.Business.Contract.Exam;
using Courses.Business.Contract.Question;
using Courses.Business.Contract.UserExam;

namespace Courses.DataAccess.Services;
public class ExamService(
    ApplicationDbContext context,
    IAnswerService answerService) : IExamService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAnswerService _answerService = answerService;

    public async Task<Result<Guid>> AddAsync(Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Modules.AnyAsync(e => e.Id == moduleId && e.CreatedById == userId, cancellationToken))
            return Result.Failure<Guid>(ModuleErrors.NotFound);

        if (await _context.Exams.AnyAsync(e => e.Title == request.Title && e.ModuleId == moduleId, cancellationToken))
            return Result.Failure<Guid>(ExamErrors.DuplicatedTitle);

        var lastModuleItemNo = await _context.ModuleItems
            .Where(e => e.ModuleId == moduleId)
            .Select(e => e.OrderIndex)
            .OrderBy(e => e)
            .LastOrDefaultAsync(cancellationToken);


        var exam = request.Adapt<Exam>();
        exam.ModuleId = moduleId;
        exam.OrderIndex = lastModuleItemNo + 1;
        exam.IsDisable = true;

        await _context.Exams.AddAsync(exam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(exam.Id);
    }
    public async Task<Result> AddExamQuestionsAsync(Guid id, Guid moduleId, string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default)
    {

        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == id, cancellationToken) is not { } exam)
            return ExamErrors.NotFoundExam;

        var courseId = await _context.Modules
            .Where(e => e.Id == moduleId && e.CreatedById == userId)
            .Select(e => e.CourseId)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (Guid.Empty == courseId)
            return ModuleErrors.NotFound;

        var existQuestions = await _context.Questions
            .Where(e => e.CourseId == courseId)
            .Select(e => new {e.IsDisable, e.Id})
            .ToListAsync(cancellationToken);

        var examQuestionValidate = await _context.ExamQuestion
            .Where(e => e.ExamId == id)
            .ToListAsync(cancellationToken);

        
        List<ExamQuestion> examQuestions = [];
        foreach (var questionId in questionIds)
        {
            if (!existQuestions.Contains(new { IsDisable = false, Id = questionId }))
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
    public async Task<Result> RemoveExamQuestionsAsync(Guid id, Guid moduleId, string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == id, cancellationToken) is not { } exam)
            return ExamErrors.NotFoundExam;

        var courseId = await _context.Modules
            .Where(e => e.Id == moduleId && e.CreatedById == userId)
            .Select(e => e.CourseId)
            .SingleOrDefaultAsync(cancellationToken);

        if (Guid.Empty == courseId)
            return ModuleErrors.NotFound;

        var existQuestions = await _context.ExamQuestion
            .Where(e => e.ExamId == id && questionIds.Contains(e.QuestionId))
            .ToListAsync(cancellationToken);

        if (existQuestions.Count != questionIds.Count())
            return QuestionErrors.NotFoundQuestionsToRemove;

        _context.ExamQuestion.RemoveRange(existQuestions);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateAsync(Guid id, Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.SingleAsync(e => e.Id == id && e.CreatedById == userId, cancellationToken) is not { } exam)
            return ExamErrors.NotFoundExam;

        if (!await _context.Exams.AnyAsync(e => e.Title == request.Title && e.ModuleId == moduleId, cancellationToken))
            return ModuleErrors.DuplicatedTitle;

        exam = request.Adapt(exam);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.FindAsync([id], cancellationToken) is not { } exam)
            return ExamErrors.NotFoundExam;

        if (exam.CreatedById !=  userId)
            return UserErrors.UnAutherizeAccess;

        if (await _context.ExamQuestion.CountAsync(e => e.ExamId == id, cancellationToken) < 10)
            return ExamErrors.NotFoundExam;

        exam.IsDisable = !exam.IsDisable;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result<ExamResponse>> GetAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        var exam = await _context.Exams
            .Select(x => new { x.Id, x.Title, x.Description, x.Duration, x.CreatedById })
            .SingleOrDefaultAsync(e => e.Id == id && e.CreatedById == userId, cancellationToken);

        if (exam is null)
            return Result.Failure<ExamResponse>(ExamErrors.NotFoundExam);

        var question = await _context.Questions
            .Include(o => o.Options)
            .Join(
                _context.ExamQuestion,
                q => q.Id,
                eq => eq.QuestionId,
                (question, _) => question.Adapt<QuestionResponse>()
            )
            .ToListAsync(cancellationToken);

        var response = new ExamResponse(exam.Id, exam.Title, exam.Description, exam.Duration, null,question);

        return Result.Success(response);
    }

    public async Task<IEnumerable<ExamResponse>> GetModuleExamsAsync(Guid moduleId, string userId, CancellationToken cancellationToken)
    {
        var courseId = await _context.Modules
            .Where(e => e.Id == moduleId)
            .Select(e => e.CourseId)
            .SingleOrDefaultAsync(cancellationToken);

        if (Guid.Empty == courseId)
            return [];

        var moduleIds = await (
            from c in _context.Courses
            join m in _context.Modules
            on c.Id equals m.CourseId
            select m.Id
            ).ToListAsync(cancellationToken);

        var exams = await (
            from e in _context.Exams
            where moduleIds.Contains(e.ModuleId)
            select new ExamResponse(e.Id, e.Title, e.Description, e.Duration, e.IsDisable, null!)
            ).ToListAsync(cancellationToken);

        return exams;
    }
    public async Task<IEnumerable<UserExamDetailResponse>> GetUserExams(Guid moduleId, string studentId ,string userId, CancellationToken cancellationToken = default)
    {
        if (!await _context.Modules.AnyAsync(e => e.Id == moduleId && e.CreatedById == userId, cancellationToken))
            return [];

        var response = await _answerService.GetAllAsync(moduleId, userId, cancellationToken);

        return response;
    }
    public async Task<IEnumerable<UserExamResponse>> GetExamUsersAsync(Guid examId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == examId && e.CreatedById == userId, cancellationToken) is not { } exam)
            return []; // not found || unautherize access

        var userExams = await _answerService.GetExamUsersAsync(exam, cancellationToken);

        return userExams;
    }
}
