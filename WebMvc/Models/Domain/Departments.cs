namespace WebMvc.Models.Domain
{
    public class Departments
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public Guid FK_facultyId { get; set; }
        public virtual Faculties FK_faculty { get; set; }
        public virtual ICollection<Courses> FK_cours { get; set; }
        public virtual ICollection<Students> FK_student { get; set; }
        public virtual ICollection<Teachers> FK_teacher { get; set; }
    }
}
