using Courses.Business.Contract.Answer;
using Courses.Business.Contract.Exam;
using Courses.Business.Contract.Question;
using Serilog.Configuration;

namespace Courses.DataAccess.Services;
public class AnswerService(ApplicationDbContext context) : IAnswerService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAnswer(int examId, string userId, IEnumerable<AnswerValues> request, CancellationToken cancellationToken = default)
    {
        if (await _context.UserExams.SingleOrDefaultAsync(e => e.UserId == userId && e.ExamId == examId && e.EndDate == null,cancellationToken) is not { } userExam)
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
    public async Task<Result<ExamResponse>> EnrollExamAsync(int examId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == examId, cancellationToken) is not { } exam)
            return Result.Failure<ExamResponse>(ExamErrors.NotFoundExam); // see error class TODO:

        if (await _context.UserExams.AnyAsync(e => e.ExamId == examId && e.UserId == userId, cancellationToken))
            return Result.Failure<ExamResponse>(UserExamErrors.DuplicatedAnswer);

        if (exam.IsDisable)
            return Result.Failure<ExamResponse>(ExamErrors.ExamedNotAvailable);

        var userExam = new UserExam
        {
            ExamId = examId,
            UserId = userId
        };

        await _context.UserExams.AddAsync(userExam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await GetEnroledExam(exam, cancellationToken);

        return Result.Success(response);
    }
    public async Task<Result<ExamResponse>> ReEnrolExamAsync(int examId, string userId, CancellationToken cancellationToken = default)
    {
        var userExams = await _context.UserExams.Where(e => e.ExamId == examId && e.UserId == userId).ToListAsync(cancellationToken);
        
        if (userExams.Count == 0)
            return Result.Failure<ExamResponse>(UserExamErrors.InvalidEnroll);
       
        if (userExams.Any(e => e.Score >= 70))
            return Result.Failure<ExamResponse>(UserExamErrors.DuplicatedAnswer);

        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == examId && !e.IsDisable, cancellationToken) is not { } exam)
            return Result.Failure<ExamResponse>(UserExamErrors.ExamNotAvailable);

        var userExam = new UserExam                                                        
        {
            ExamId = examId,
            UserId = userId
        };

        await _context.UserExams.AddAsync(userExam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await GetEnroledExam(exam, cancellationToken);

        return Result.Success(response); 
    }
    public async Task<IEnumerable<UserExamResponse>> UserExamsAsync(int examId, string userId, CancellationToken cancellationToken)
    {
        var userExams = await (
            from e in _context.Exams
            join ue in _context.UserExams.Where(e => e.UserId == userId)
            on e.Id equals ue.ExamId
            where e.Id == examId
            select new
            {
                ue.ExamId,
                e.Title,
                e.Description,
                e.Duration,
                e.NoQuestion,
                userExamId = ue.Id,
                userDeuration = ue.Duration,
                ue.Score,
                ue.StartDate,
                ue.EndDate
            }
            ).ToListAsync(cancellationToken);


        var question = await (
            from q in _context.Questions
            join eq in _context.ExamQuestion
            on q.Id equals eq.QuestionId
            join a in _context.Answers
            on q.Id equals a.QuestionId
            where eq.ExamId == examId
            select new
            {
                question = new UserQuestionResponse( 
                    q.Id, 
                    q.Text, 
                    a.OptionId == q.Options.Where(e => e.IsCorrect).Select(e => e.Id).First(), 
                    a.OptionId, 
                    q.Options.Adapt<List<UserOptionResponse>>()
                    ),
                a.UserExamId
            }).ToListAsync(cancellationToken);


        var response = 
            from ux in userExams
            join q in question
            on ux.userExamId equals q.UserExamId into questions
            select new UserExamResponse(
                ux.ExamId,
                ux.Title,
                ux.Description,
                ux.Duration,
                ux.userDeuration,
                ux.StartDate,
                ux.EndDate,
                ux.Score,
                questions.Select(e => e.question).ToList()
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
                q.Options.Adapt<List<OptionResponse>>(),
                null
                )
            ).ToListAsync(cancellationToken);

        ExamResponse response = new(exam.Id, exam.Title, exam.Description, exam.Duration, questions, null);

        return response;
    }
}
