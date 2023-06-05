namespace WebMvc.Models.ViewModels
{
    public class VM_DowloadVideo
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public Guid FK_coursesId { get; set; }
        public IFormFile video { get; set; }
    }
}
