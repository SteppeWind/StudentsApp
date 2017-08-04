using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.WEB.Models;
using StudentsApp.WEB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    public class AccountController : Controller
    {

        private IUserService UserService;
        private IStudentService StudentService;
        private ITeacherService TeacherService;
        private IDeanService DeanService;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(
            IUserService userService,
            IStudentService studentService,
            ITeacherService teacherService,
            IDeanService deanService)
        {
            UserService = userService;
            StudentService = studentService;
            TeacherService = teacherService;
            DeanService = deanService;
        }

        #region Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var claim = await UserService.Authenticate(new PersonDTO()
                {
                    Email = loginModel.Email,
                    Password = loginModel.Password
                });
                
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties()
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(loginModel);
        }

        #endregion 

        [HttpGet]
        [Authorize(Roles = "teacher, dean, admin")]
        public ActionResult Edit(int idPerson = 1, string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            var personVM = BaseViewModel.UniversalConvert<StudentDTO, PersonViewModel>(StudentService.Get(idPerson));
            return View(personVM);
        }


        [HttpPost]
        [Authorize(Roles = "teacher, dean, admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonViewModel person, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var studentDTO = BaseViewModel.UniversalReverseConvert<PersonViewModel, StudentDTO>(person);
                StudentService.Update(studentDTO);
                TempData["message"] = "Изменения были сохранены";
                return Redirect(returnUrl);
            }
            else
            {
                return View();
            }
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> Details()
        {
            if (User.IsInRole("student"))
            {
                var student = await StudentService.GetByEmailAsync(User.Identity.Name);
                return RedirectToAction("Details", "Student", new { id = student.Id });
            }

            if (User.IsInRole("teacher"))
            {
                var teacher = await TeacherService.GetByEmailAsync(User.Identity.Name);
                return RedirectToAction("Details", "Teacher", new { id = teacher.Id });
            }

            if (User.IsInRole("dean"))
            {
                var dean = await DeanService.GetByEmailAsync(User.Identity.Name);
                return RedirectToAction("Details", "Dean", new { id = dean.Id });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}