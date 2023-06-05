using WebMvc.Models.Domain;

namespace WebMvc.Models.ViewModels
{
    public class VM_Department
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string FK_Name { get; set; }
    }
}
