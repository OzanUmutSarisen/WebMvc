using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using WebMvc.Data;
using WebMvc.Models.Domain;
using WebMvc.Models.StudentViewModels;
using WebMvc.Models.ViewModels;

namespace WebMvc.Funtions
{
    public class StudentsFuntions
    {
        public List<SVM_Course> VM_Models(Guid SId, DbSet<CourseStudent> DB1, DbSet<Courses> DB2, DbSet<Teachers> DB3)
        {
            var corseIds = DB1.Where(x => x.FK_StudentId == SId).ToList();

            List<Courses> courses = new List<Courses>();
            foreach (var model in corseIds)
            {

                courses.Add(DB2.Where(x => x.Id == model.FK_CourseId).FirstOrDefault());
            }
            List<SVM_Course> svm_courses = new List<SVM_Course>();
            foreach (var model in courses)
            {
                svm_courses.Add(
                    new SVM_Course()
                    {
                        Id = model.Id,
                        name = model.name,
                        FK_Name = DB3.Where(x => x.Id == model.FK_teacherId).FirstOrDefault().name
                    });
            }

            return svm_courses;
        }
        public List<SVM_Video> VM_Models(Guid CId, DbSet<Videos> DB1, DbSet<Courses> DB2)
        {
            var videos = DB1.Where(x => x.FK_coursesId == CId).ToList();
            var courseName = DB2.Where(x => x.Id == CId).FirstOrDefault().name;

            List<SVM_Video> svm_videos = new List<SVM_Video>();
            foreach (var video in videos)
            {
                svm_videos.Add(new SVM_Video()
                {
                    Id = video.Id,
                    name = video.name,
                    FK_Name = courseName
                });
            }

            return svm_videos;
        }
        public List<SVM_Answer> VM_Models(Guid SID, Guid VID, DbSet<Questions> DB1, DbSet<Answers> DB2)
        {

            List<Answers> studentAnswers = DB2.Where(x=> x.FK_studentId == SID).ToList();
            List<Questions> videoQuestions = DB1.Where(x=> x.FK_videoId == VID).ToList();

            List<SVM_Answer> answers = new List<SVM_Answer>();

            foreach (var question in videoQuestions)
            {
                foreach(var answer in studentAnswers) 
                {
                    if(answer.FK_questionId == question.Id)
                        answers.Add(new SVM_Answer()
                            {
                                Id = answer.Id,
                                quest = question.quest,
                                studentAnswer = answer.answer,
                                trueAnswers = question.answer
                            });
                }

            }

            return answers;

        }

    }
}
