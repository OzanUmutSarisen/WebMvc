namespace WebMvc.Models.Domain
{
    public class CourseStudent
    {
        public Guid Id { get; set; }
        public Guid FK_StudentId { get; set; }
        public Guid FK_CourseId { get; set; }

        public virtual Courses FK_Course { get; set; }
        public virtual Students FK_Student { get; set; }
    }
}
