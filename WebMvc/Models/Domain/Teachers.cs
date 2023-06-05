
namespace WebMvc.Models.Domain
{
    public class Teachers
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public Guid FK_departmentId { get; set; }
        public virtual Departments FK_department { get; set; }
        public virtual ICollection<Courses> FK_course { get; set; }
        public virtual ICollection<Accesses> FK_access { get; set; }
    }
}
