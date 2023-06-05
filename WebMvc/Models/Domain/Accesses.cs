namespace WebMvc.Models.Domain
{
    public class Accesses
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public virtual ICollection<Teachers> FK_teacher { get; set; }
    }
}
