
namespace Courses.DataAccess.Presistence.EntitiesConfigurations;
public class UploadFileConfiguration : IEntityTypeConfiguration<UploadedFile>
{
    public void Configure(EntityTypeBuilder<UploadedFile> builder)
    {
        builder.Property(e => e.FileName)
            .HasMaxLength(250);
        
        builder.Property(e => e.StoredFileName)
            .HasMaxLength(250);

        builder.Property(e => e.ContentType)
            .HasMaxLength(100);
        
        builder.Property(e => e.FileExtension)
            .HasMaxLength(100);
    }
}
