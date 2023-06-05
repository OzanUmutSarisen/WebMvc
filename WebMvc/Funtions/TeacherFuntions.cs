using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebMvc.Data;
using WebMvc.Models.Domain;
using WebMvc.Models.StudentViewModels;
using WebMvc.Models.TeacherViewModels;

namespace WebMvc.Funtions
{
    public class TeacherFuntions
    {
        public List<TVM_StudentResult> VM_Models(Guid VID, DbSet<Questions> DB1, DbSet<Answers> DB2, DbSet<Students> DB3)
        {

            var videoQuestions = DB1.Where(x => x.FK_videoId == VID).ToList();
            var allStudentAnswers = DB2.ToList();

            List<TVM_StudentResult> results = new List<TVM_StudentResult>();

            foreach (var question in videoQuestions)
            {
                foreach (var answer in allStudentAnswers)
                {
                    if (answer.FK_questionId == question.Id)
                        results.Add(new TVM_StudentResult()
                        {
                            Id = answer.Id,
                            question = question.quest,
                            trueAnswer = question.answer,
                            studentAnswer = answer.answer,
                            studentName = DB3.Where(x=>x.Id == answer.FK_studentId).First().name,
                        });
                }

            }

            return results;

        }
    }
}
