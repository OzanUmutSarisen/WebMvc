using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using WebMvc.Data;
using WebMvc.Funtions;
using WebMvc.Models.Domain;
using WebMvc.Models.TeacherViewModels;
using WebMvc.Models.ViewModels;
using WebMvc.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebMvc.Models;

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
            Videos video;
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

                video = new Videos()
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

                video = new Videos()
                {
                    Id = Id,
                    name = videoRequest.name,
                    location = filePath,
                    startTime = videoRequest.startTime,
                    FK_coursesId = videoRequest.FK_coursesId
                };

                var course = sqlDataBaseContext.Courses.FirstOrDefault(c => c.Id == videoRequest.FK_coursesId);


                if (course != null)
                {

                    string connectionString = "Server=127.0.0.1;Port=3306;Database=moodle_db;user=root;password=;";
                    int databaseId = 0;
                    int fieldId = 0;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT `id` FROM `mdl_data` WHERE `name`=@courseId;";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@courseId", course.name);

                            databaseId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        connection.Close();
                    }

                    if (databaseId > 0)
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "SELECT `id` FROM `mdl_data_fields` WHERE `dataid`=@databaseId;";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@databaseId", databaseId);

                                fieldId = Convert.ToInt32(command.ExecuteScalar());
                            }

                            connection.Close();
                        }

                        if (fieldId > 0)
                        {
                            var url = "http://localhost/moodle/webservice/rest/server.php?wstoken=97c552d00ab0f67506e74b1c7d485892&wsfunction=mod_data_add_entry&databaseid=" + databaseId + "&data[1][fieldid]=" + fieldId + "&data[1][subfield]=0&data[1][value]=\"https://localhost:5001/student/StudentWatchVideo?id="+ Id + "\"";

                            using (var client = new HttpClient())
                            {

                                var response = await client.PostAsync(url, null);

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseContent = await response.Content.ReadAsStringAsync();
                                    Console.WriteLine("Giriş başarıyla eklendi.");
                                    Console.WriteLine("Yanıt içeriği: " + responseContent);
                                }
                                else
                                {
                                    Console.WriteLine("Hata oluştu. Yanıt kodu: " + response.StatusCode);
                                }

                            }
                        }
                    }
                }

                await sqlDataBaseContext.Videos.AddAsync(video);
                await sqlDataBaseContext.SaveChangesAsync();
            }

            return Redirect("TeacherAddQuestions?id=" + video.Id);
        }

        [HttpGet]
        public async Task<IActionResult> TeacherAddQuestions()
        {
            Guid Id = new Guid(Request.Query["id"]);
            ViewBag.VideoId = Id;
            ViewBag.CourseId = sqlDataBaseContext.Videos.FirstOrDefault(c => c.Id == Id).FK_coursesId;

            string vttData = System.IO.File.ReadAllText("D:\\Users\\ozanu\\source\\repos\\WebMvc\\WebMvc\\wwwroot\\static\\ExampleSubtitle.vtt");
            var subtitles = teacherFuntions.ParseVttData(vttData);


            return View(subtitles);

            /*if (sqlDataBaseContext.Questions.Where(x => x.Id == Id).FirstOrDefault() != null)
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

            return View(new Questions());*/
        }

        [HttpPost]
        public async Task<IActionResult> TakeTeacherAddQuestions(string sentence, string selectedWord, string time, string videoId)
        {
            Questions question = new Questions
            {
                Id = Guid.NewGuid(),
                quest = sentence,
                answer = selectedWord,
                questionTime = TimeSpan.Parse(teacherFuntions.AddSecondsToTime(time, 3)),
                FK_videoId = new Guid(videoId)
            };

            await sqlDataBaseContext.Questions.AddAsync(question);
            await sqlDataBaseContext.SaveChangesAsync();

            return Json(new { message = "ok" });
        }

        [HttpGet]
        public async Task<IActionResult> TeacherVideoWatch()
        {
            var Id = new Guid(Request.Query["id"]);
            ViewBag.TeacherId = TempData["user"];

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
