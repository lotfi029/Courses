using Courses.Business.Contract.Answer;
using System.Runtime.Serialization;

namespace Courses.DataAccess.Services;
public class AnswerService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAnswer(int examId, string userId, IEnumerable<AnswerValues> request, CancellationToken cancellationToken = default)
    {
        if (await _context.UserExams.SingleOrDefaultAsync(e => e.UserId == userId && e.ExamId == examId,cancellationToken) is not { } userExam)
            return QuestionErrors.NotFoundOptions; // exam not found


        var examQuestionid = await _context.ExamQuestion
            .Where(e => e.ExamId == examId)
            .Select(e => e.QuestionId)
            .ToListAsync(cancellationToken);

        if (examQuestionid.Count == 0 || request.Select(e => e.QuestionId) != examQuestionid)
            return QuestionErrors.NotFoundQuestion; // wrong in selected question
        
        float degree = 100 / examQuestionid.Count;

        var options = await _context.Options
            .Where(e => examQuestionid.Contains(e.QuestionId) && e.IsCorrect)
            .Select(e => new AnswerValues (e.QuestionId, e.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (options.Count != examQuestionid.Count)
            return QuestionErrors.NotFoundOptions; // wrong in options

        float score = 0;
        foreach(var question in request)
        {
            if (options.Contains(question))
                score += degree;
        }

        userExam.Score = score;
        userExam.EndDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> EnrollExamAsync(int examId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Exams.AnyAsync(e => e.Id == examId, cancellationToken))
            return ExamErrors.NotFoundExam; // see error class TODO:

        var userExam = new UserExam
        {
            ExamId = examId,
            UserId = userId
        };

        await _context.UserExams.AddAsync(userExam, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
