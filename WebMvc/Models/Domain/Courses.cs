namespace WebMvc.Models.Domain
{
    public class Courses
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public Guid FK_teacherId { get; set; }
        public Guid FK_departmentId { get; set; }
        public virtual Teachers FK_teacher { get; set; }
        public virtual Departments FK_department { get; set; }
        public virtual ICollection<Videos> FK_videos { get; set; }
        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
    }
}
