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
        public async Task<ActionResult> Edit(string idPerson = "", string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            var personVM = BaseViewModel.UniversalConvert<PersonDTO, PersonViewModel>(await UserService.Get(idPerson));
            return View(personVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonViewModel person, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var profile = BaseViewModel.UniversalReverseConvert<PersonViewModel, PersonDTO>(person);
                var result = await UserService.UpdateProfile(profile);

                if (result.Succedeed)
                {
                    TempData["message"] = result.Message;
                }
                else
                {
                    TempData["errorMessage"] = result.Message;
                    return View(person);
                }
            }
            else
            {
                return View(person);
            }

            return Redirect(returnUrl);
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            string Id = User.Identity.GetUserId();

            if (User.IsInRole("student"))
            {
                return RedirectToAction("Details", "Student", new { id = Id });
            }

            if (User.IsInRole("teacher"))
            {
                return RedirectToAction("Details", "Teacher", new { id = Id });
            }

            if (User.IsInRole("dean"))
            {
                return RedirectToAction("Details", "Dean", new { id = Id });
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