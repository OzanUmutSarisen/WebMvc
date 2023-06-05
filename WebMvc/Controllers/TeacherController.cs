using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebMvc.Data;
using WebMvc.Funtions;
using WebMvc.Models.Domain;
using WebMvc.Models.TeacherViewModels;
using WebMvc.Models.ViewModels;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SqlDataBaseContext sqlDataBaseContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private TeacherFuntions teacherFuntions = new TeacherFuntions();

        public TeacherController(SqlDataBaseContext sqlDataBaseContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {

            this._hostingEnvironment = hostingEnvironment;
            this.sqlDataBaseContext = sqlDataBaseContext;
        }

        [HttpGet]
        public async Task<IActionResult> TeacherHome()
        {
            Guid Id = ViewBag.Id = TempData["User"];
            TempData["User"] = Id;

            return View(sqlDataBaseContext.Courses.Where(x=>x.FK_teacherId == Id).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> CourseInfo()
        {
            var courseId = new Guid(Request.Query["id"]);
            var Id = ViewBag.Id = TempData["User"];
            TempData["User"] = Id;

            return View(sqlDataBaseContext.Videos.Where(x => x.FK_coursesId == courseId).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> TeacherStudentResult()
        {
            var videoId = new Guid(Request.Query["id"]);
            var Id = ViewBag.Id = TempData["User"];
            TempData["User"] = Id;

            return View(teacherFuntions.VM_Models(videoId, sqlDataBaseContext.Questions, sqlDataBaseContext.Answers, sqlDataBaseContext.Students));
        }

        [HttpGet]
        public async Task<IActionResult> TeacherVideoQuestions()
        {
            var videoId = new Guid(Request.Query["id"]);

            return View(sqlDataBaseContext.Questions.Where(x => x.FK_videoId == videoId).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> TeacherAddVideos()
        {
            Guid Id = new Guid(Request.Query["id"]);
            if (sqlDataBaseContext.Videos.Where(x=>x.Id == Id).FirstOrDefault() != null)
            {
                ViewBag.CourseId = sqlDataBaseContext.Videos.Where(x => x.Id == Id).FirstOrDefault().FK_coursesId;
                ViewBag.VideoId = Id;
            }
            else
            {
                ViewBag.VideoId = new Guid("00000000-0000-0000-0000-000000000000");
                ViewBag.CourseId = Id;
            }

            

            return View(new TVM_Videos());
        }

        [HttpPost]
        public async Task<IActionResult> TeacherAddVideos(TVM_Videos videoRequest)
        {
            if (videoRequest.Id != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                var Id = videoRequest.Id;

                string filePath = _hostingEnvironment.ContentRootPath + "/wwwroot/static/" + Id.ToString() + ".mp4";
                if (videoRequest != null)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await videoRequest.video.CopyToAsync(stream);
                    }
                }

                var video = new Videos()
                {
                    Id = Id,
                    name = videoRequest.name,
                    location = filePath,
                    startTime = videoRequest.startTime,
                    FK_coursesId = videoRequest.FK_coursesId
                };

                sqlDataBaseContext.Update(video);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                var Id = Guid.NewGuid();
                string filePath = _hostingEnvironment.ContentRootPath + "/wwwroot/static/" + Id.ToString() + ".mp4";
                if (videoRequest != null)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await videoRequest.video.CopyToAsync(stream);
                    }
                }

                var video = new Videos()
                {
                    Id = Id,
                    name = videoRequest.name,
                    location = filePath,
                    startTime = videoRequest.startTime,
                    FK_coursesId = videoRequest.FK_coursesId
                };

                await sqlDataBaseContext.Videos.AddAsync(video);
                await sqlDataBaseContext.SaveChangesAsync();
                
            }
            return Redirect("CourseInfo?Id=" + videoRequest.FK_coursesId);

        }

        [HttpGet]
        public async Task<IActionResult> TeacherAddQuestions()
        {
            Guid Id = new Guid(Request.Query["id"]);
            if (sqlDataBaseContext.Questions.Where(x => x.Id == Id).FirstOrDefault() != null)
            {
                ViewBag.VideoId = sqlDataBaseContext.Questions.Where(x => x.Id == Id).FirstOrDefault().FK_videoId;
                ViewBag.QuestionId = Id;
            }
            else
            {
                ViewBag.QuestionId = new Guid("00000000-0000-0000-0000-000000000000");
                ViewBag.VideoId = Id;
            }

            //var data = await sqlDataBaseContext.Questions.FindAsync(new Guid(Id));

            return View(new Questions());
        }

        [HttpPost]
        public async Task<IActionResult> TeacherAddQuestions(Questions questionRequest)
        {
            if (questionRequest.Id != null)
            {
                sqlDataBaseContext.Update(questionRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                questionRequest.Id = Guid.NewGuid();
                await sqlDataBaseContext.Questions.AddAsync(questionRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }

            return Redirect("TeacherVideoQuestions?Id=" + questionRequest.FK_videoId);
        }

        [HttpGet]
        public async Task<IActionResult> TeacherVideoWatch()
        {
            var Id = new Guid(Request.Query["id"]);
            ViewBag.TeacherId = TempData["user"];

            var videos = await sqlDataBaseContext.Videos.FindAsync(Id);
            var questions = await sqlDataBaseContext.Questions.Where(x => x.FK_videoId == videos.Id).ToListAsync();

            var VMIndex = new VM_WatchVideo
            {
                name = videos.Id.ToString() + ".mp4",
                Question = questions
            };
            return View(VMIndex);
        }

        [HttpPost]
        public async Task<IActionResult> TakeAnswer(string studentAnswer, string questionId, string studentId)
        {
            var answer = new Answers()
            {
                Id = Guid.NewGuid(),
                answer = studentAnswer,
                FK_questionId = new Guid(questionId),
                FK_studentId = new Guid(studentId)

            };
            await sqlDataBaseContext.Answers.AddAsync(answer);
            await sqlDataBaseContext.SaveChangesAsync();
            return Json(new { message = "ok" });
        }

        [HttpGet]
        public async Task<IActionResult> TeacherDeleteVideo()
        {
            Guid Id = new Guid(Request.Query["id"]);

            if (await sqlDataBaseContext.Videos.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Videos.FindAsync(Id);
                sqlDataBaseContext.Videos.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return Redirect("CourseInfo?Id="+data.FK_coursesId);
            }

            if (await sqlDataBaseContext.Questions.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Questions.FindAsync(Id);
                sqlDataBaseContext.Questions.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return Redirect("TeacherVideoQuestions?Id="+data.FK_videoId);
            }

            return RedirectToAction("TeacherHome");
        }
    }
}
