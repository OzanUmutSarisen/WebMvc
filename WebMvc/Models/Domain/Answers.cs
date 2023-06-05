namespace WebMvc.Models.Domain
{
    public class Answers
    {
        public Guid Id { get; set; }
        public string answer { get; set; }
        public Guid FK_questionId { get; set; }
        public Guid FK_studentId { get; set; }
        public virtual Questions FK_question { get; set; }
        public virtual Students FK_student { get; set; }
    }
}
