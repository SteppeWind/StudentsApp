using Microsoft.Owin.Security;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    public class HomeController : Controller
    {
        private ITeacherService TeacherService;
        private IUserService UserService;
        private IStudentService StudentService;
        private IDeanService DeanService;
        private IStartService StartService;


        public HomeController(
            ITeacherService teacherService,
            IStudentService studentService,
            IDeanService deanService,
            IUserService userService,
            IStartService startService)
        {
            TeacherService = teacherService;
            UserService = userService;
            DeanService = deanService;
            StudentService = studentService;
            StartService = startService;
        }

        private async Task roles()
        {
            await UserService.SetInitialData(new PersonDTO()
            {
                Email = "admin@mail.ru",
                Name = "Михаил",
                MiddleName = "Александрович",
                Surname = "Берлиоз",
                Password = "массолит",
                Role = "admin"
            }, new List<string>() { "student", "teacher", "dean", "admin" });

            foreach (var item in StudentService.GetAll)
            {
                item.Role = "student";
                item.Password = "qwe123";
                await UserService.Create(item);
            }
            foreach (var item in TeacherService.GetAll)
            {
                item.Role = "teacher";
                item.Password = "qwe123";
                await UserService.Create(item);
            }
            foreach (var item in DeanService.GetAll)
            {
                item.Role = "dean";
                item.Password = "qwe123";
                await UserService.Create(item);
            }
        }

        public ActionResult Index()
        {            
            return View();
        }

        public async Task<ActionResult> Set()
        {
            //var teacher = TeacherService.Get(48);
            //TeacherService.FullRemove(teacher.Id);

            //StartService.FillData();
            //await roles();
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult GetCountStudentAndTeacher()
        {
            int countStudents = StudentService.GetAll.Count();
            int countTeachers = TeacherService.GetAll.Count();

            return PartialView(new Tuple<int, int>(countStudents, countTeachers));
        }
    }
}