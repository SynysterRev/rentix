using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.Entities;
using System.Reflection.Emit;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.DocumentType)
                   .IsRequired();

            builder.Property(d => d.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(d => d.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(d => d.Description)
                   .HasMaxLength(500);

            builder.Property(d => d.EntityType)
                   .HasConversion<string>();

            builder.Property(d => d.UploadAt)
                   .HasDefaultValueSql("NOW()")
                   .IsRequired();

            builder.HasOne(d => d.Property)
                   .WithMany(p => p.Documents)
                   .HasForeignKey(d => d.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
