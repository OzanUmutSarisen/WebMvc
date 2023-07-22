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
using MySql.Data.MySqlClient;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Google.Protobuf.WellKnownTypes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MySqlX.XDevAPI.Common;
using WebMvc.Models.TeacherViewModels;
using System.IO;
using System;
using System.Xml.Linq;
using WebMvc.Funtions;

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

        TeacherFuntions teacherFuntions = new TeacherFuntions();

        [HttpPost]
        public IActionResult timepost(string[] words)
        {
            string vttData = System.IO.File.ReadAllText("D:\\Users\\ozanu\\source\\repos\\WebMvc\\WebMvc\\wwwroot\\static\\ExampleSubtitle.vtt");
            var subtitles = teacherFuntions.ParseVttData(vttData);

            List<Questions> questions = new List<Questions>();

            foreach (var word in words)
            {
                foreach (var subtitle in subtitles)
                {
                    if (subtitle.Sentence.Contains(word) == true)
                    {
                        var modifiedSentence = subtitle.Sentence.Replace(word, "____________");

                        Questions question = new Questions
                        {
                            Id = Guid.NewGuid(),
                            quest = modifiedSentence,
                            answer = word,
                            questionTime = TimeSpan.Parse(teacherFuntions.AddSecondsToTime(subtitle.Time, 3))
                        };

                        questions.Add(question);
                    }


                }

            }
            return View(questions);
        }

    }
}

