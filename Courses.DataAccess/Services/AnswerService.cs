using Courses.Business.Contract.Answer;
using System.Runtime.Serialization;

namespace Courses.DataAccess.Services;
public class AnswerService(ApplicationDbContext context) : IAnswerService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAnswer(int examId, string userId, IEnumerable<AnswerValues> request, CancellationToken cancellationToken = default)
    {
        if (await _context.UserExams.SingleOrDefaultAsync(e => e.UserId == userId && e.ExamId == examId,cancellationToken) is not { } userExam)
            return UserExamErrors.InvalidSubmitExam; // exam not found

        if (userExam.Score >= 70 && userExam.EndDate is not null)
            return UserExamErrors.DuplicatedAnswer;

        var examQuestionid = await _context.ExamQuestion
            .Where(e => e.ExamId == examId)
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

        List<UserAnswer> answers = [];
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

        await _context.Answers.AddRangeAsync(answers, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> EnrollExamAsync(int examId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.SingleOrDefaultAsync(e => e.Id == examId, cancellationToken) is not { } exam)
            return ExamErrors.NotFoundExam; // see error class TODO:

        if (exam.IsDisable)
            return ExamErrors.DuplicatedTitle;

        var userExam = new UserExam
        {
            ExamId = examId,
            UserId = userId
        };

        await _context.UserExams.AddAsync(userExam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ReEnrolExamAsync(int examId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.UserExams.Where(e => examId == e.ExamId && e.UserId == userId).ToListAsync(cancellationToken) is not { } userExams)
            return UserExamErrors.DuplicatedAnswer;

        if (userExams.Any(e => e.Score >= 70))
            return UserExamErrors.DuplicatedAnswer;

        if (await _context.Exams.AnyAsync(e => e.Id == examId && !e.IsDisable, cancellationToken))
            return UserExamErrors.ExamNotAvailable;

        var userExam = new UserExam
        {
            ExamId = examId,
            UserId = userId
        };

        return Result.Success();
    }
}
