namespace WebMvc.Models.Domain
{
    public class Students
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string password { get; set; }
        public string tc { get; set; }
        public Guid FK_departmentId { get; set; }
        public virtual Departments FK_department { get; set; }
        public virtual ICollection<Answers> FK_Answer { get; set; }
        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
    }
}
