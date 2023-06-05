namespace WebMvc.Models.ViewModels
{
    public class VM_Video
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public DateTime startTime { get; set; }
        public string FK_Name { get; set; }
    }
}
