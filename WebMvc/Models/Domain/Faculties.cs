namespace WebMvc.Models.Domain
{
    public class Faculties
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public virtual ICollection<Departments> FK_department { get; set; }
    }
}
