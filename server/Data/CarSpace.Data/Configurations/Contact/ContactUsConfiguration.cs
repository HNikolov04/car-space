using CarSpace.Data.Models.Entities.Contact;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Configurations.Contact;

public class ContactUsConfiguration : IEntityTypeConfiguration<ContactUs>
{
    public void Configure(EntityTypeBuilder<ContactUs> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(a => a.Message)
            .IsRequired()
            .HasMaxLength(1000);
    }
}
