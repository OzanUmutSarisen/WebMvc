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
            var Id = new Guid("e13b91f7-6b35-40c5-32af-08db711f33ab");
            ViewBag.TeacherId = TempData["user"];
            ViewBag.VideoId = Id;

            string vttData = System.IO.File.ReadAllText("D:\\Users\\ozanu\\source\\repos\\WebMvc\\WebMvc\\wwwroot\\static\\ExampleSubtitle.vtt");
            var subtitles = ParseVttData(vttData);


            return View(subtitles);
        }

        [HttpPost]
        public IActionResult timepost(string sentence, string selectedWord, string time)
        {
            var newSentence = new TVM_SelectQuestionWord
            {
                Sentence = sentence,
                SelectedWord = selectedWord,
                Time = AddSecondsToTime(time, 3)
            };

            return Json(new { message = "ok" });
        }
        public string AddSecondsToTime(string timeString, int seconds)
        {
            TimeSpan time = TimeSpan.Parse(timeString);
            time = time.Add(TimeSpan.FromSeconds(seconds));
            return time.ToString(@"hh\:mm\:ss\.fff");
        }

        public static List<TVM_SubSentence> ParseVttData(string vttData)
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

    }
}

