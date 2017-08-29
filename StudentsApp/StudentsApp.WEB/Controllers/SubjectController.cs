using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles = "dean, admin")]
    public class SubjectController : Controller
    {
        ISubjectService SubjectService;
        IFacultyService FacultyService;
        ITeacherService TeacherService;
        ITeacherSubjectService TeacherSubjectService;

        public SubjectController(
            ISubjectService subjectService,
            ITeacherService teacherService,
            IFacultyService facultyService,
            ITeacherSubjectService teacherSubjectService)
        {
            FacultyService = facultyService;
            SubjectService = subjectService;
            TeacherSubjectService = teacherSubjectService;
            TeacherService = teacherService;
        }

        [HttpGet]
        public ActionResult Create(string idFaculty, string returnUrl)
        {
            RegisterSubject subjectVM = new RegisterSubject() { FacultyId = idFaculty };

            var teachersVM = GetTeachers(idFaculty);
            subjectVM.Teachers = teachersVM.ToList();

            return View(subjectVM);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterSubject subject, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var subjectDTO = BaseViewModel.UniversalReverseConvert<RegisterSubject, SubjectDTO>(subject);
                var result = await SubjectService.AddAsync(subjectDTO);

                var tempDataMessage = TempData["message"] = "";
                var tempDataErrorMessage = TempData["errorMessage"] = "";
                string message = result.Message + "/n";

                if (result.Succedeed)
                {
                    subjectDTO = SubjectService.GetAll.Last();

                    foreach (var item in subject.SelectedIdTeachers)
                    {
                        result = await TeacherSubjectService.AddAsync(new TeacherSubjectDTO()
                        {
                            TeacherId = item,
                            SubjectId = subject.Id
                        });
                        message = result.Message + "/n";

                        if (result.Succedeed)
                        {
                            tempDataMessage += message;
                        }
                        else
                        {
                            tempDataErrorMessage += message;
                        }
                    }

                    return Redirect(returnUrl);
                }
                else
                {
                    tempDataErrorMessage = message;
                }
            }

            subject.Teachers = GetTeachers(subject.FacultyId).ToList();
            return View(subject);
        }


        public IEnumerable<TeacherViewModel> GetTeachers(string idFaculty)
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