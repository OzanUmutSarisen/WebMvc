using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMvc.Models.Domain;

namespace WebMvc.Data.Mapping
{
    public class CourseStudentMapping : IEntityTypeConfiguration<CourseStudent>
    {
        public void Configure(EntityTypeBuilder<CourseStudent> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.FK_Course)
                .WithMany(x => x.CourseStudent)
                .HasForeignKey(x => x.FK_CourseId);

            builder
                .HasOne(x => x.FK_Student)
                .WithMany(x => x.CourseStudent)
                .HasForeignKey(x => x.FK_StudentId);
        }
    }
}
