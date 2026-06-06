using System;
using Data.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class StudentDocumentConfiguration : IEntityTypeConfiguration<StudentDocument>
{
    public void Configure(EntityTypeBuilder<StudentDocument> builder)
    {
        builder.ToTable("student_documents");


        builder.HasKey(sd => sd.Id);
        builder.Property(sd => sd.Id)
               .HasColumnName("id")
               .HasDefaultValueSql("nextval('\"student_document_sequence\"')")

        ;
        builder.Property(sd => sd.DocumentName).IsRequired().HasMaxLength(100);
        builder.Property(sd => sd.DocumentUrl).IsRequired().HasMaxLength(200);
        builder.Property(sd => sd.UploadedAt).IsRequired();
        builder.Property(sd => sd.StudentId).IsRequired();

        builder.HasOne(sd => sd.Student)
               .WithMany(s => s.Documents)
               .HasForeignKey(sd => sd.StudentId).HasPrincipalKey(s => s.Id)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
