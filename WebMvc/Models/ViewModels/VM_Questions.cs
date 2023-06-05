namespace WebMvc.Models.ViewModels
{
    public class VM_Questions
    {
        public Guid Id { get; set; }
        public string quest { get; set; }
        public string answer { get; set; }
        public TimeSpan questionTime { get; set; }
        public string FK_Name { get; set; }
    }
}
