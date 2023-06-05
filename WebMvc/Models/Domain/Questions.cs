namespace WebMvc.Models.Domain
{
    public class Questions
    {
        public Guid Id { get; set; }
        public string quest { get; set; }
        public string answer { get; set; }
        public TimeSpan questionTime { get; set; }
        public Guid FK_videoId { get; set; }
        public virtual Videos FK_video { get; set; }

    }
}
