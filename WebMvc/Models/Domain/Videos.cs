namespace WebMvc.Models.Domain
{
    public class Videos
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public DateTime startTime { get; set; }
        public Guid FK_coursesId { get; set; }
        public virtual Courses FK_courses { get; set; }
        public virtual ICollection<Questions> FK_questions { get; set; }
    }
}
