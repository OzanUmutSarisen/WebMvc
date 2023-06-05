using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebMvc.Data;
using WebMvc.Models.Domain;
using WebMvc.Models.ViewModels;

namespace WebMvc.Funtions
{
    public class AdminFuntions
    {
        //DropDownList Maker
        public List<SelectListItem> DropDownListValues(DbSet<Faculties> DB)
        {
            List<SelectListItem> dropDownListValues = (from i in DB.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = i.Id.ToString(),
                                                           Text = i.name
                                                       }).ToList();
            return dropDownListValues;
        }
        public List<SelectListItem> DropDownListValues(DbSet<Departments> DB)
        {
            List<SelectListItem> dropDownListValues = (from i in DB.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = i.Id.ToString(),
                                                           Text = i.name
                                                       }).ToList();
            return dropDownListValues;
        }
        public List<SelectListItem> DropDownListValues(DbSet<Students> DB)
        {
            List<SelectListItem> dropDownListValues = (from i in DB.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = i.Id.ToString(),
                                                           Text = i.name
                                                       }).ToList();
            return dropDownListValues;
        }
        public List<SelectListItem> DropDownListValues(DbSet<Teachers> DB)
        {
            List<SelectListItem> dropDownListValues = (from i in DB.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = i.Id.ToString(),
                                                           Text = i.name
                                                       }).ToList();
            return dropDownListValues;
        }
        public List<SelectListItem> DropDownListValues(DbSet<Courses> DB)
        {
            List<SelectListItem> dropDownListValues = (from i in DB.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = i.Id.ToString(),
                                                           Text = i.name
                                                       }).ToList();
            return dropDownListValues;
        }
        public List<SelectListItem> DropDownListValues(DbSet<Videos> DB)
        {
            List<SelectListItem> dropDownListValues = (from i in DB.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = i.Id.ToString(),
                                                           Text = i.name
                                                       }).ToList();
            return dropDownListValues;
        }
        public List<SelectListItem> DropDownListValues(DbSet<Questions> DB)
        {
            List<SelectListItem> dropDownListValues = (from i in DB.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = i.Id.ToString(),
                                                           Text = i.quest
                                                       }).ToList();
            return dropDownListValues;
        }


        //VM_Model Maker
        public List<VM_Department> VM_Models(DbSet<Departments> DB1, DbSet<Faculties> DB2)
        {
            List<VM_Department> data = new List<VM_Department>();
            foreach (var obj in DB1.ToList())
            {
                data.Add(new VM_Department()
                {
                    Id = obj.Id,
                    name = obj.name,
                    FK_Name = DB2.Where(x => x.Id == obj.FK_facultyId).FirstOrDefault().name
                }
                    );
            }
            return data;
        }
        public List<VM_Student> VM_Models(DbSet<Students> DB1, DbSet<Departments> DB2)
        {
            List<VM_Student> data = new List<VM_Student>();
            foreach (var obj in DB1.ToList())
            {
                data.Add(new VM_Student()
                {
                    Id = obj.Id,
                    name = obj.name,
                    number= obj.number,
                    tc= obj.tc,
                    FK_Name = DB2.Where(x => x.Id == obj.FK_departmentId).FirstOrDefault().name
                }
                    );
            }
            return data;
        }
        public List<VM_Teacher> VM_Models(DbSet<Teachers> DB1, DbSet<Departments> DB2)
        {
            List<VM_Teacher> data = new List<VM_Teacher>();
            foreach (var obj in DB1.ToList())
            {
                data.Add(new VM_Teacher()
                {
                    Id = obj.Id,
                    name = obj.name,
                    userName= obj.userName,
                    password= obj.password,
                    FK_Name = DB2.Where(x => x.Id == obj.FK_departmentId).FirstOrDefault().name
                }
                    );
            }
            return data;
        }
        public List<VM_Course> VM_Models(DbSet<Courses> DB1, DbSet<Teachers> DB2, DbSet<Departments> DB3)
        {
            List<VM_Course> data = new List<VM_Course>();
            foreach (var obj in DB1.ToList())
            {
                data.Add(new VM_Course()
                {
                    Id = obj.Id,
                    name = obj.name,
                    FK_Name1 = DB2.Where(x => x.Id == obj.FK_teacherId).FirstOrDefault().name,
                    FK_Name2 = DB3.Where(x => x.Id == obj.FK_departmentId).FirstOrDefault().name
                }
                    );
            }
            return data;
        }
        public List<VM_Video> VM_Models(DbSet<Videos> DB1, DbSet<Courses> DB2)
        {
            List<VM_Video> data = new List<VM_Video>();
            foreach (var obj in DB1.ToList())
            {
                data.Add(new VM_Video()
                {
                    Id = obj.Id,
                    name = obj.name,
                    location = obj.location,
                    startTime = obj.startTime,
                    FK_Name = DB2.Where(x => x.Id == obj.FK_coursesId).FirstOrDefault().name
                }
                    );
            }
            return data;
        }
        public List<VM_Questions> VM_Models(DbSet<Questions> DB1, DbSet<Videos> DB2)
        {
            List<VM_Questions> data = new List<VM_Questions>();
            foreach (var obj in DB1.ToList())
            {
                data.Add(new VM_Questions()
                {
                    Id = obj.Id,
                    quest = obj.quest,
                    questionTime= obj.questionTime,
                    answer=obj.answer,
                    FK_Name = DB2.Where(x => x.Id == obj.FK_videoId).FirstOrDefault().name
                }
                    );
            }
            return data;
        }
        public List<VM_Answer> VM_Models(DbSet<Answers> DB1, DbSet<Students> DB2, DbSet<Questions> DB3, DbSet<Admin> DB4)
        {
            List<VM_Answer> data = new List<VM_Answer>();
            foreach (var obj in DB1.ToList())
            {
                data.Add(new VM_Answer()
                {
                    Id = obj.Id,
                    answer = obj.answer,
                    FK_Name1 = AnswerName(obj.FK_studentId, DB2, DB4),
                    FK_Name2 = DB3.Where(x => x.Id == obj.FK_questionId).FirstOrDefault().quest
                }
                    );
            }
            return data;
        }

        public string AnswerName(Guid SID, DbSet<Students> DB1,  DbSet<Admin> DB2)
        {
            if(DB1.Where(x => x.Id == SID).FirstOrDefault() != null)
            {
                return DB1.Where(x => x.Id == SID).FirstOrDefault().name;
            }else
            {
                return DB2.Where(x => x.Id == SID).FirstOrDefault().name;
            }
        }

        public List<CourseStudent> addStudentToCourses(Guid SId, Guid SDId, DbSet<Courses> DB1, DbSet<CourseStudent> DB2)
        {
            List<Courses> Courses = DB1.Where(x => x.FK_departmentId == SDId).ToList();

            List<CourseStudent> studentHave = new List<CourseStudent>();

            foreach(var course in Courses)
            {
                studentHave.Add(new CourseStudent()
                {
                    Id = new Guid(),
                    FK_StudentId = SId,
                    FK_CourseId = course.Id
                });
            }

            return studentHave;

        }

    }
}
