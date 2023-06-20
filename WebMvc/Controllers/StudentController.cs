using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMvc.Data;
using WebMvc.Funtions;
using WebMvc.Models.Domain;
using WebMvc.Models.StudentViewModels;
using WebMvc.Models.ViewModels;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class StudentController : Controller
    {
        private readonly SqlDataBaseContext sqlDataBaseContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private StudentsFuntions studentFuntions = new StudentsFuntions();

        public StudentController(SqlDataBaseContext sqlDataBaseContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {

            this._hostingEnvironment = hostingEnvironment;
            this.sqlDataBaseContext = sqlDataBaseContext;
        }

        [HttpGet]
        public IActionResult StudentHome()
        {
            Guid Id = ViewBag.Id = TempData["User"];
            TempData["User"] = Id;
            ViewBag.student = sqlDataBaseContext.Students.Where(x => x.Id == Id).First();
            return View(studentFuntions.VM_Models(Id, sqlDataBaseContext.CoursesStudents, sqlDataBaseContext.Courses, sqlDataBaseContext.Teachers));
        }

        [HttpGet]
        public IActionResult CourseInfo()
        {
            var Id = ViewBag.Id = TempData["User"];
            TempData["User"] = Id;
            var courseId = new Guid(Request.Query["id"]);

            return View(studentFuntions.VM_Models(courseId, sqlDataBaseContext.Videos, sqlDataBaseContext.Courses));
        }

        [HttpGet]
        public IActionResult StudentResult()
        {
            var Id = ViewBag.Id = TempData["User"];
            TempData["User"] = Id;
            var VideoId = new Guid(Request.Query["id"]);

            return View(studentFuntions.VM_Models(Id, VideoId, sqlDataBaseContext.Questions, sqlDataBaseContext.Answers));
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var Id = new Guid(Request.Query["id"]);

            if (await sqlDataBaseContext.Faculties.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Faculties.FindAsync(Id);
                sqlDataBaseContext.Faculties.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return RedirectToAction("Faculties");
            }

            if (await sqlDataBaseContext.Departments.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Departments.FindAsync(Id);
                sqlDataBaseContext.Departments.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return RedirectToAction("Departments");
            }

            if (await sqlDataBaseContext.Students.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Students.FindAsync(Id);
                sqlDataBaseContext.Students.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return RedirectToAction("Students");
            }

            if (await sqlDataBaseContext.Teachers.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Teachers.FindAsync(Id);
                sqlDataBaseContext.Teachers.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return RedirectToAction("Teachers");
            }

            if (await sqlDataBaseContext.Courses.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Courses.FindAsync(Id);
                sqlDataBaseContext.Courses.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return RedirectToAction("Courses");
            }

            if (await sqlDataBaseContext.Videos.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Videos.FindAsync(Id);
                sqlDataBaseContext.Videos.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return RedirectToAction("Videos");
            }

            if (await sqlDataBaseContext.Questions.FindAsync(Id) != null)
            {
                var data = await sqlDataBaseContext.Questions.FindAsync(Id);
                sqlDataBaseContext.Questions.Remove(data);
                await sqlDataBaseContext.SaveChangesAsync();
                return RedirectToAction("Questions");
            }

            return RedirectToAction("AdminHome");
        }

        [HttpGet]
        public async Task<IActionResult> StudentWatchVideo()
        {
            var Id = new Guid(Request.Query["id"]);
            var studentId = ViewBag.studentId = TempData["User"];
            TempData["User"] = studentId;

            if (studentId == null)
            {
                TempData["VideoId"] = Id;
                return RedirectToAction("Login");
            }

            var videos = await sqlDataBaseContext.Videos.FindAsync(Id);
            var questions = await sqlDataBaseContext.Questions.Where(x => x.FK_videoId == videos.Id).ToListAsync();
            var sortedquestionsList = questions.OrderBy(q => q.questionTime).ToList();

            var VMIndex = new VM_WatchVideo
            {
                name = videos.Id.ToString() + ".mp4",
                Question = sortedquestionsList
            };
            return View(VMIndex);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin userRequest)
        {
            if (sqlDataBaseContext.Students.Where(x => x.number == userRequest.name & x.password == userRequest.password).Count() > 0)
            {

                TempData["User"] = sqlDataBaseContext.Students.Where(x => x.number == userRequest.name & x.password == userRequest.password).FirstOrDefault().Id;
                return Redirect("../student/StudentWatchVideo?id=" + TempData["VideoId"].ToString());

            }
            else
            {
                return RedirectToAction("Login");

            }


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

    }
}
