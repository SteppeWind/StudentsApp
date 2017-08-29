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
            DeanService = deanService;
            StudentService = studentService;
            StartService = startService;
            UserService = userService;            
        }

        private async Task SetData()
        {
            StartService.ClearData();
            await UserService.SetRoles("student", "teacher", "dean", "admin");
            await StartService.FillDataAsync();
        }

        public async Task<ActionResult> Index()
        {
            if (!StartService.IsExistDB)
            {
                await SetData();
            }
            return View();
        }

        public async Task<ActionResult> Set()
        {
            await SetData();

            //TeacherService.FullRemove((await TeacherService.GetByEmailAsync("navalnyblat@mail.ru")).Id);

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
            int countStudents = StudentService.Count;
            int countTeachers = TeacherService.Count;

            return PartialView(new Tuple<int, int>(countStudents, countTeachers));
        }
    }
}