using CarSpace.Data.Models.Entities.CarForum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Configurations.CarForum;

public class CarForumCommentConfiguration : IEntityTypeConfiguration<CarForumComment>
{
    public void Configure(EntityTypeBuilder<CarForumComment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithMany(u => u.CreatedCarForumComments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.CarForumArticle)
            .WithMany(a => a.CarForumComments)
            .HasForeignKey(c => c.CarForumArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
