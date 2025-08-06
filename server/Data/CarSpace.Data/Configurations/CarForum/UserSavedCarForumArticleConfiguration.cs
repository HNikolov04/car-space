using CarSpace.Data.Models.Entities.CarForum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Configurations.CarForum;

public class UserSavedCarForumArticleConfiguration : IEntityTypeConfiguration<UserSavedCarForumArticle>
{
    public void Configure(EntityTypeBuilder<UserSavedCarForumArticle> builder)
    {
        builder.HasKey(x => new { x.UserId, x.CarForumArticleId });

        builder.HasOne(x => x.User)
            .WithMany(u => u.SavedCarForumArticles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CarForumArticle)
            .WithMany(a => a.SavedByUsers)
            .HasForeignKey(x => x.CarForumArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
