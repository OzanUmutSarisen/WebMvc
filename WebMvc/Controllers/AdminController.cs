using Microsoft.AspNetCore.Mvc;
using WebMvc.Data;
using WebMvc.Models.Domain;
using WebMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMvc.Models.AddModels;
using Microsoft.AspNetCore.Hosting.Server;
using WebMvc.Models.ViewModels;
using WebMvc.Funtions;
using System.Linq;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly SqlDataBaseContext sqlDataBaseContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private AdminFuntions adminFuntions = new AdminFuntions();

        public AdminController(SqlDataBaseContext sqlDataBaseContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {

            this._hostingEnvironment= hostingEnvironment;
            this.sqlDataBaseContext = sqlDataBaseContext;
        }


        
        [HttpGet]
        public async Task<IActionResult> AdminHome()
        {
            ViewBag.Id = TempData["User"];
            TempData["User"] = ViewBag.Id;
            return View();
        }

        // Tables View Pages

        [HttpGet]
        public IActionResult Faculties()
        {
            return View(sqlDataBaseContext.Faculties.ToList());
        }

        [HttpGet]
        public IActionResult Departments()
        {
            return View(adminFuntions.VM_Models(sqlDataBaseContext.Departments, sqlDataBaseContext.Faculties));
        }

        [HttpGet]
        public IActionResult Students()
        {
            return View(adminFuntions.VM_Models(sqlDataBaseContext.Students, sqlDataBaseContext.Departments));
        }

        [HttpGet]
        public IActionResult Teachers()
        {
            return View(adminFuntions.VM_Models(sqlDataBaseContext.Teachers, sqlDataBaseContext.Departments));
        }

        [HttpGet]
        public IActionResult Courses()
        {
            return View(adminFuntions.VM_Models(sqlDataBaseContext.Courses, sqlDataBaseContext.Teachers, sqlDataBaseContext.Departments));
        }

        [HttpGet]
        public IActionResult Videos()
        {
            return View(adminFuntions.VM_Models(sqlDataBaseContext.Videos, sqlDataBaseContext.Courses));
        }

        [HttpGet]
        public IActionResult Questions()
        {
            return View(adminFuntions.VM_Models(sqlDataBaseContext.Questions, sqlDataBaseContext.Videos));
        }

        [HttpGet]
        public IActionResult Answers()
        {
            return View(adminFuntions.VM_Models(sqlDataBaseContext.Answers, sqlDataBaseContext.Students, sqlDataBaseContext.Questions, sqlDataBaseContext.Admins));
        }


        // Add data to tables
        [HttpGet]
        public async Task<IActionResult> AddFaculties()
        {
            var Id = Request.Query["id"];
            if ( Id.Count == 0)
            {
                return View(new Faculties());
            }
            var data = await sqlDataBaseContext.Faculties.FindAsync(new Guid(Id));
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddFaculties(Faculties facultyRequest)
        {
            /*var faculty = new Faculties()
            {
                Id = Guid.NewGuid(),
                name = facultyRequest.name
            };*/
            if(facultyRequest.Id != null)
            {
                sqlDataBaseContext.Update(facultyRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                facultyRequest.Id = Guid.NewGuid();
                await sqlDataBaseContext.Faculties.AddAsync(facultyRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }

            return RedirectToAction("Faculties");
        }

        [HttpGet]
        public async Task<IActionResult> AddDepartments()
        {
            var Id = Request.Query["id"];
            ViewBag.ddl = adminFuntions.DropDownListValues(sqlDataBaseContext.Faculties);

            if (Id.Count == 0)
            {
                return View(new Departments());
            }
            var data = await sqlDataBaseContext.Departments.FindAsync(new Guid(Id));
            
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartments(Departments departmentRequest)
        {
            /*var faculty = sqlDataBaseContext.Faculties.Where(m => m.Id == departmentRequest.FK_facultyId).FirstOrDefault();
            var department = new Departments()
            {
                Id = Guid.NewGuid(),
                name = departmentRequest.name,
                FK_facultyId = faculty.Id
            };*/
            if (departmentRequest.Id != null)
            {
                sqlDataBaseContext.Update(departmentRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                departmentRequest.Id = Guid.NewGuid();
                await sqlDataBaseContext.Departments.AddAsync(departmentRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            return RedirectToAction("Departments");
        }


        [HttpGet]
        public async Task<IActionResult> AddStudents()
        {
            ViewBag.ddl = adminFuntions.DropDownListValues(sqlDataBaseContext.Departments);

            var Id = Request.Query["id"];
            if (Id.Count == 0)
            {
                return View(new Students());
            }
            var data = await sqlDataBaseContext.Students.FindAsync(new Guid(Id));

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudents(Students studentsRequest)
        {
            /*var department = sqlDataBaseContext.Departments.Where(m => m.Id == studentsRequest.FK_departmentId).FirstOrDefault();
            var student = new Students()
            {
                Id = Guid.NewGuid(),
                name = studentsRequest.name,
                number= studentsRequest.number,
                password= studentsRequest.password,
                tc = studentsRequest.tc,
                FK_departmentId = department.Id
            };*/
            if (studentsRequest.Id != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                sqlDataBaseContext.Update(studentsRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                studentsRequest.Id = Guid.NewGuid();
                await sqlDataBaseContext.Students.AddAsync(studentsRequest);
                await sqlDataBaseContext.SaveChangesAsync();
                var studentCourses = adminFuntions.addStudentToCourses(studentsRequest.Id, studentsRequest.FK_departmentId, sqlDataBaseContext.Courses, sqlDataBaseContext.CoursesStudents);
                foreach(var studentCourse in studentCourses)
                {
                    await sqlDataBaseContext.CoursesStudents.AddAsync(studentCourse);
                    await sqlDataBaseContext.SaveChangesAsync();
                }
                
            }

            return RedirectToAction("Students");
        }

        [HttpGet]
        public async Task<IActionResult> AddTeachers()
        {
            ViewBag.ddl = adminFuntions.DropDownListValues(sqlDataBaseContext.Departments);

            var Id = Request.Query["id"];
            if (Id.Count == 0)
            {
                return View(new Teachers());
            }
            var data = await sqlDataBaseContext.Teachers.FindAsync(new Guid(Id));

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeachers(Teachers teachersRequest)
        {
            /*var department = sqlDataBaseContext.Departments.Where(m => m.Id == teachersRequest.FK_departmentId).FirstOrDefault();
            var teacher = new Teachers()
            {
                Id = Guid.NewGuid(),
                name = teachersRequest.name,
                password = teachersRequest.password,
                userName = teachersRequest.userName,
                FK_departmentId = department.Id
            };*/
            if (teachersRequest.Id != null)
            {
                sqlDataBaseContext.Update(teachersRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                teachersRequest.Id = Guid.NewGuid();
                await sqlDataBaseContext.Teachers.AddAsync(teachersRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }

            return RedirectToAction("Teachers");
        }

        [HttpGet]
        public async Task<IActionResult> AddCourses()
        {
            ViewBag.ddl = adminFuntions.DropDownListValues(sqlDataBaseContext.Teachers);
            ViewBag.ddl2 = adminFuntions.DropDownListValues(sqlDataBaseContext.Departments);

            var Id = Request.Query["id"];
            if (Id.Count == 0)
            {
                return View(new Courses());
            }
            var data = await sqlDataBaseContext.Courses.FindAsync(new Guid(Id));

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourses(Courses courseRequest)
        {
            if (courseRequest.Id != null)
            {
                sqlDataBaseContext.Update(courseRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                courseRequest.Id = Guid.NewGuid();
                await sqlDataBaseContext.Courses.AddAsync(courseRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }


            if(sqlDataBaseContext.Students.Where(x => x.FK_departmentId == courseRequest.FK_departmentId) != null)
            {
                List<Students> studentHaveCorse = sqlDataBaseContext.Students.Where(x => x.FK_departmentId == courseRequest.FK_departmentId).ToList();
                foreach(Students student in studentHaveCorse)
                {
                    CourseStudent coursesStudents = new CourseStudent()
                    {
                        FK_CourseId = courseRequest.Id,
                        FK_StudentId = student.Id
                    };

                    await sqlDataBaseContext.CoursesStudents.AddAsync(coursesStudents);
                    await sqlDataBaseContext.SaveChangesAsync();
                }
            }
            

            return RedirectToAction("Courses");
        }


        [HttpGet]
        public async Task<IActionResult> AddVideos()
        {;
            ViewBag.ddl = adminFuntions.DropDownListValues(sqlDataBaseContext.Courses);

            var Id = Request.Query["id"];
            if (Id.Count == 0)
            {
                return View(new VM_DowloadVideo());
            }
            var data = await sqlDataBaseContext.Videos.FindAsync(new Guid(Id));
            var data2 = new VM_DowloadVideo()
            {
                Id = data.Id,
                name = data.name,
                startTime = data.startTime,
                FK_coursesId =data.FK_coursesId
            };

            return View(data2);
        }

        [HttpPost]
        public async Task<IActionResult> AddVideos(VM_DowloadVideo videoRequest)
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

            return RedirectToAction("addvideos");
        }

        [HttpGet]
        public async Task<IActionResult> AddQuestions()
        {
            ViewBag.ddl = adminFuntions.DropDownListValues(sqlDataBaseContext.Videos);

            var Id = Request.Query["id"];
            if (Id.Count == 0)
            {
                return View(new Questions());
            }
            var data = await sqlDataBaseContext.Questions.FindAsync(new Guid(Id));

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestions(Questions questionRequest)
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
                
            return RedirectToAction("Questions");
        }

        [HttpGet]
        public async Task<IActionResult> AddAnswers()
        {
            ViewBag.ddl1 = adminFuntions.DropDownListValues(sqlDataBaseContext.Questions);
            ViewBag.ddl2 = adminFuntions.DropDownListValues(sqlDataBaseContext.Students);

            var Id = Request.Query["id"];
            if (Id.Count == 0)
            {
                return View(new Answers());
            }
            var data = await sqlDataBaseContext.Answers.FindAsync(new Guid(Id));

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnswers(Answers answerRequest)
        {
            if (answerRequest.Id != null)
            {
                sqlDataBaseContext.Update(answerRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }
            else
            {
                answerRequest.Id = Guid.NewGuid();
                await sqlDataBaseContext.Answers.AddAsync(answerRequest);
                await sqlDataBaseContext.SaveChangesAsync();
            }

            return RedirectToAction("Answers");
        }

        //Delete Funtions

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
        public async Task<IActionResult> WatchVideo()
        {
            var Id = new Guid(Request.Query["id"]);
            ViewBag.adminId = TempData["user"];

            var videos = await sqlDataBaseContext.Videos.FindAsync(Id);
            var questions = await sqlDataBaseContext.Questions.Where(x => x.FK_videoId == videos.Id).ToListAsync();

            var VMIndex = new VM_WatchVideo
            {
                name = videos.Id.ToString()+".mp4",
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
