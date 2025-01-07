using Courses.Business.Contract.Exam;

namespace Courses.DataAccess.Services;
public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<int>> AddQuestionsAsync(string userId, Guid courseId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Courses.AnyAsync(e => e.Id == courseId && e.CreatedById == userId, cancellationToken))
            return Result.Failure<int>(QuestionErrors.NotFoundQuestion); // TODO: the sut errr message

        Question question = new()
        {
            Text = request.Text,
            CourseId = courseId
        };

        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Id);
    }
    public async Task<Result> UpdateQuestionsAsync(int id, Guid courseId, string userId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Courses.AnyAsync(e => e.Id == courseId && e.CreatedById == userId, cancellationToken))
            return CourseErrors.NotFound; // TODO: the sut errr message

        var rowsUpdated = await _context.Questions
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Text, request.Text),
                cancellationToken
            );

        return rowsUpdated == 0 ? QuestionErrors.NotFoundQuestion : Result.Success();
    }
    public async Task<Result> RemoveQuestionAsync(int id, Guid courseId, string userId, CancellationToken cancellationToken = default)
    {
        if (!await _context.Courses.AnyAsync(e => e.Id == courseId && e.CreatedById == userId, cancellationToken))
            return CourseErrors.NotFound; // TODO: the sut errr message

        var rowsDeleted = await _context.Questions
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted == 0 ? QuestionErrors.NotFoundQuestion : Result.Success();
    }
    public async Task<Result<List<QuestionResponse>>> GetAllQuestionAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        var questions = await _context.Questions
            .Where(e => e.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var response = questions.Adapt<List<QuestionResponse>>();

        return Result.Success(response);
    }
    public async Task<Result> AddQuestionOptionsAsync(int questionId, Guid courseId, string userId, OptionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Courses.AnyAsync(e => e.Id == courseId && e.CreatedById == userId, cancellationToken))
            return CourseErrors.NotFound; // TODO: the sut errr message

        if (await _context.Questions.Include(e => e.Options).SingleOrDefaultAsync(e => e.Id == questionId, cancellationToken) is not { } question)
            return QuestionErrors.NotFoundQuestion;


        if (question.Options.Count > 0)
            return QuestionErrors.DuplicatedOptions;

        bool isValid = IsValidOptions(request);

        if (!isValid)
            return QuestionErrors.InvalidOptions;


        List<Option> options = [];
        foreach (var option in request.Options)
            options.Add(new()
            {
                IsCorrect = option.IsCorrect,
                QuestionId = questionId,
                Text = option.Text,
            });
        question.IsDisable = false;

        await _context.Options.AddRangeAsync(options, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateQuestionOptionsAsync(int questionId, Guid courseId, string userId, OptionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Courses.AnyAsync(e => e.Id == courseId && e.CreatedById == userId, cancellationToken))
            return CourseErrors.NotFound; // TODO: the sut errr message

        bool isValid = IsValidOptions(request);

        var rowsUpdated = await _context.Options
           .Where(e => e.QuestionId == questionId)
           .ExecuteDeleteAsync(cancellationToken);

        var result = await AddQuestionOptionsAsync(questionId, courseId, userId, request, cancellationToken);

        return result;
    }
    public async Task<Result> ToggleIsDisableAsync(int questionId, Guid courseId, string userId, CancellationToken cancellationToken = default)
    {
        if (!await _context.Courses.AnyAsync(e => e.Id == courseId && e.CreatedById == userId, cancellationToken))
            return CourseErrors.NotFound; // TODO: the sut errr message

        var rowsUpdated = await _context.Questions
            .Where(e => e.Id == questionId)
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.IsDisable, e => !e.IsDisable),
                cancellationToken
            );
        
        return rowsUpdated == 0 ? QuestionErrors.NotFoundQuestion : Result.Success();
    }
    public async Task<Result<QuestionResponse>> GetQuestionAsync(int questionId, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Include(e => e.Options)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == questionId, cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.NotFoundQuestion);

        var response = question.Adapt<QuestionResponse>();

        return Result.Success(response);
    }
    private static bool IsValidOptions(OptionRequest request)
    {
        return request.Options.Count >= 2 && request.Options.Count <= 5
            && request.Options.Count(e => e.IsCorrect == true) == 1
            && request.Options.Select(e => e.Text).Distinct(StringComparer.OrdinalIgnoreCase).Count() == request.Options.Select(e => e.Text).Count();
    }
}
