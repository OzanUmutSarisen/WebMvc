namespace WebMvc.Models.Domain
{
    public class Subtitles
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public Guid FK_videoId { get; set; }
        public virtual Videos FK_video { get; set; }
    }
}
