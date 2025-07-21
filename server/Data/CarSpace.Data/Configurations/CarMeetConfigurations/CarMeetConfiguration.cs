using CarSpace.Data.Models.Entities.Meet;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Configurations.CarMeetConfigurations;

public class CarMeetConfiguration : IEntityTypeConfiguration<CarMeet>
{
    public void Configure(EntityTypeBuilder<CarMeet> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(m => m.MeetDate)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasOne(m => m.Creator)
            .WithMany(u => u.CreatedMeets)
            .HasForeignKey(m => m.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(m => m.Participants)
            .WithMany(u => u.JoinedMeets)
            .UsingEntity<Dictionary<string, object>>(
                "CarMeetParticipants",
                j => j
                    .HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("ParticipantsId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<CarMeet>()
                    .WithMany()
                    .HasForeignKey("JoinedMeetsId")
                    .OnDelete(DeleteBehavior.Cascade)); 
    }
}