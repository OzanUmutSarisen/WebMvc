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

        public List<TVM_SubSentence> ParseVttData(string vttData)
        {
            List<TVM_SubSentence> subtitles = new List<TVM_SubSentence>();

            string[] lines = vttData.Split("\n");

            string currentSentence = string.Empty;
            string currentTime = string.Empty;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (line.StartsWith("00:") && line.Contains("-->"))
                {
                    if (!string.IsNullOrEmpty(currentSentence) && !string.IsNullOrEmpty(currentTime))
                    {
                        TVM_SubSentence subtitle = new TVM_SubSentence
                        {
                            Sentence = currentSentence,
                            Time = currentTime
                        };

                        subtitles.Add(subtitle);
                    }

                    currentTime = line.Split(new[] { "-->" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                    currentSentence = string.Empty;
                }
                else if (!string.IsNullOrEmpty(line))
                {
                    currentSentence += line + " ";
                }
            }

            // Son satırın altyazıyı içermesi durumunda eklemeyi unutmamak için kontrol yapılır
            if (!string.IsNullOrEmpty(currentSentence) && !string.IsNullOrEmpty(currentTime))
            {
                TVM_SubSentence subtitle = new TVM_SubSentence
                {
                    Sentence = currentSentence,
                    Time = currentTime
                };

                subtitles.Add(subtitle);
            }

            return subtitles;
        }

        public string AddSecondsToTime(string timeString, int seconds)
        {
            TimeSpan time = TimeSpan.Parse(timeString);
            time = time.Add(TimeSpan.FromSeconds(seconds));
            return time.ToString(@"hh\:mm\:ss\.fff");
        }
    }
}
