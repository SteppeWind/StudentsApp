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
    interface IA
    {
        int GetId { get; }
    }    

    class A : IA
    {
        public int GetId => throw new NotImplementedException();
    }

    class B
    {
        A SomeProp { get; set; }

        public B()
        {
            SomeProp = new A();
        }
    }

    class C
    {
        IA Field;

        public C(IA param)
        {
            Field = param;
        }
    }    

    class Ninject
    {
        public static T Create<T>() where T : class
        {
            2
        }
    }


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

                string message = result.Message + "<br>";

                if (result.Succedeed)
                {
                    TempData["message"] = message;

                    foreach (var item in subject.SelectedIdTeachers)
                    {
                        result = TeacherSubjectService.AddBySubjectName(subjectDTO.SubjectName, new TeacherSubjectDTO()
                        {
                            TeacherId = item
                        });
                        message = result.Message + "<br>";

                        if (result.Succedeed)
                        {
                            TempData["message"] += message;
                        }
                        else
                        {
                            TempData["errorMessage"] += message;
                        }
                    }

                    return Redirect(returnUrl);
                }
                else
                {
                    TempData["errorMessage"] = message;
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