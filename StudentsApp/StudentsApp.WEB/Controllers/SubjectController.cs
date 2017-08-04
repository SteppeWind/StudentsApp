using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles ="dean, admin")]
    public class SubjectController : Controller
    {
        ISubjectService SubjectService;
        IFacultyService FacultyService;
        ITeacherService TeacherService;

        public SubjectController(
            ISubjectService subjectService,
            ITeacherService teacherService,
            IFacultyService facultyService)
        {
            FacultyService = facultyService;
            SubjectService = subjectService;
            TeacherService = teacherService;
        }

        [HttpGet]
        public ActionResult Create(int idFaculty, string returnUrl)
        {            
            RegisterSubject subjectVM = new RegisterSubject() { FacultyId = idFaculty };

            var teachersVM = GetTeachers(idFaculty);
            subjectVM.Teachers = teachersVM.ToList();

            return View(subjectVM);
        }

        [HttpPost]
        public ActionResult Create(RegisterSubject subject, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var subjectDTO = BaseViewModel.UniversalReverseConvert<RegisterSubject, SubjectDTO>(subject);
                    SubjectService.Add(subjectDTO);

                    foreach (var item in subject.SelectedIdTeachers)
                    {
                        TeacherService.AddSubject(SubjectService.GetAll.Last().Id, int.Parse(item));
                    }

                    return Redirect(returnUrl);
                }
                catch (ValidationException ex)
                {
                    TempData["errorMessage"] = ex.Message;
                }
            }

            subject.Teachers = GetTeachers(subject.FacultyId).ToList();

            return View(subject);
        }


        public IEnumerable<TeacherViewModel> GetTeachers(int idFaculty)
        {
            return BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>(TeacherService.GetTeachers(idFaculty));
        }

        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }
    }
}