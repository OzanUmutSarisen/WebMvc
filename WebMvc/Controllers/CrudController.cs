using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using WebMvc.Data;
using WebMvc.Models;
using WebMvc.Models.Domain;
using WebMvc.Models.ViewModels;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CrudController : Controller
    {
        private readonly SqlDataBaseContext sqlDataBaseContext;

        public CrudController(SqlDataBaseContext sqlDataBaseContext)
        {
            this.sqlDataBaseContext = sqlDataBaseContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = await sqlDataBaseContext.Questions.ToListAsync();
            var sTeacherDate = "20.10.2022 16:10:30";
            var sLocalDate = DateTime.Now.ToString();

            var dtTeachersDate = DateTime.Parse(sTeacherDate);
            var dtLocalDate = DateTime.Parse(sLocalDate);

            var tsDifTime = dtTeachersDate - dtLocalDate;
            var dTotalSecond = tsDifTime.TotalSeconds;

            


            if (dTotalSecond >= 0)
            {
                var VMIndex = new VM_WatchVideoWithWaiting
                {
                    //WaitToStart = dTotalSecond,
                    //StartTime = 0,
                    //StudentNo = TempData["StudentNumber"].ToString(),
                    Question = questions
                };
                return View(VMIndex);
            }
            else
            {
                var VMIndex = new VM_WatchVideoWithWaiting
                {
                   // WaitToStart = 0,
                    //StartTime = Math.Abs(dTotalSecond),
                    //StudentNo = TempData["StudentNumber"].ToString(),
                    Question = questions,
                };
                return View(VMIndex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> TakeAnswer(string studentAnswer, int studentQuestionCount, string studentNumber)
        {
            var answer = new Answers()
            {
                Id = Guid.NewGuid(),
                answer = studentAnswer,
            };
            await sqlDataBaseContext.Answers.AddAsync(answer);
            await sqlDataBaseContext.SaveChangesAsync();
            return Json(new { message = "ok" });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddQuestionViewModel questRequest)
        {
            var question = new Questions()
            {
                Id = Guid.NewGuid(),
                quest = questRequest.quest,
                answer = questRequest.answer,
                //startTime = questRequest.startTime
            };
            await sqlDataBaseContext.Questions.AddAsync(question);
            await sqlDataBaseContext.SaveChangesAsync();
            return RedirectToAction("Add");
        }

        [HttpPost]
        public IActionResult Finish()
        {
            return View();
        }

        public IActionResult time()
        {


            return View();
        }
    }
}