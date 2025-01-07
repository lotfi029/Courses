namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.HasKey(e => new { e.ExamId, e.QuestionId });
    }
}
