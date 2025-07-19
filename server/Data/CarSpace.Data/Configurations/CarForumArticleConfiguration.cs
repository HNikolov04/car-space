using CarSpace.Data.Models.CarForum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpace.Data.Configurations;

public class CarForumArticleConfiguration : IEntityTypeConfiguration<CarForumArticle>
{
    public void Configure(EntityTypeBuilder<CarForumArticle> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(a => a.Brand)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(a => a.Comments)
               .WithOne(c => c.CarForumArticle)
               .HasForeignKey(c => c.CarForumArticleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
