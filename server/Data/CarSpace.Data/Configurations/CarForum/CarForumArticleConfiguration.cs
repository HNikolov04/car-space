using CarSpace.Data.Models.CarForum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpace.Data.Configurations.CarForum;

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

        builder.Property(a => a.UpdatedAt)
            .IsRequired();

        builder.HasOne(a => a.User)
            .WithMany(u => u.CreatedCarForumArticles)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.CarBrand)
            .WithMany(b => b.CarForumArticles)
            .HasForeignKey(a => a.CarBrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.CarForumComments)
            .WithOne(c => c.CarForumArticle)
            .HasForeignKey(c => c.CarForumArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.SavedByUsers)
            .WithOne(s => s.CarForumArticle)
            .HasForeignKey(s => s.CarForumArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}