using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Diagnostics;
using WebMvc.Data;
using WebMvc.Models;
using WebMvc.Models.Domain;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SqlDataBaseContext sqlDataBaseContext;

        public HomeController(ILogger<HomeController> logger, SqlDataBaseContext sqlDataBaseContext)
        {
            _logger = logger;
            this.sqlDataBaseContext = sqlDataBaseContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin userRequest)
        {
            if (sqlDataBaseContext.Admins.Where(x => x.name == userRequest.name & x.password == userRequest.password).Count() > 0) {

                TempData["User"] = sqlDataBaseContext.Admins.Where(x => x.name == userRequest.name & x.password == userRequest.password).FirstOrDefault().Id;
                return Redirect("../Admin/AdminHome");

            }
            else if(sqlDataBaseContext.Teachers.Where(x => x.userName == userRequest.name & x.password == userRequest.password).Count() > 0){

                TempData["User"] = sqlDataBaseContext.Teachers.Where(x => x.userName == userRequest.name & x.password == userRequest.password).FirstOrDefault().Id;
                return Redirect("../Teacher/TeacherHome"); 

            }
            else if(sqlDataBaseContext.Students.Where(x => x.number == userRequest.name & x.password == userRequest.password).Count() > 0){

                TempData["User"] = sqlDataBaseContext.Students.Where(x => x.number == userRequest.name & x.password == userRequest.password).FirstOrDefault().Id;
                return Redirect("../Student/StudentHome");

            }
            else
            {
                return RedirectToAction("Login");

            }
            
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}