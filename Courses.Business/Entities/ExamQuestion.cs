namespace Courses.Business.Entities;
public class ExamQuestion : IEquatable<ExamQuestion>
{
    public int ExamId { get; set; }
    public int QuestionId { get; set; }
    public Exam Exam { get; set; } = default!;
    public Question Question { get; set; } = default!;

    public override bool Equals(object? obj)
        => base.Equals(obj as ExamQuestion);

    public bool Equals(ExamQuestion? other)
        => other?.ExamId == ExamId && other?.QuestionId == QuestionId;

    public static bool operator ==(ExamQuestion? left, ExamQuestion? right)
        => left?.ExamId == right?.ExamId && left?.QuestionId == right?.QuestionId;
    
    public static bool operator !=(ExamQuestion? left, ExamQuestion? right)
        => left?.ExamId != right?.ExamId || left?.QuestionId != right?.QuestionId;

    public override int GetHashCode()
        => (ExamId, QuestionId).GetHashCode();
}
