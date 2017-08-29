using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles = "teacher, dean, admin")]
    public class TeacherSubjectController : Controller
    {
        private IDeanService DeanService;
        private ITeacherService TeacherService;
        private IFacultyService FacultyService;
        private ISubjectService SubjectService;
        private ITeacherSubjectService TeacherSubjectService;
        private IStudentSubjectService StudentSubjectService;


        public TeacherSubjectController(
            IDeanService deanService,
            ITeacherService teacherService,
            IFacultyService facultyService,
            ISubjectService subjectService,
            ITeacherSubjectService teacherSubjectService,
            IStudentSubjectService studentSubjectService)
        {
            DeanService = deanService;
            TeacherService = teacherService;
            FacultyService = facultyService;
            SubjectService = subjectService;
            StudentSubjectService = studentSubjectService;
            TeacherSubjectService = teacherSubjectService;
        }

        // GET: TeacherSubject
        public ActionResult Index()
        {
            return View();
        }

        // GET: TeacherSubject
        public ActionResult Index(Func<TeacherSubjectDTO, bool> predicate)
        {
            return View();
        }

        private TeacherSubjectViewModel GetTeacherSubject(string id)
        {
            TeacherSubjectViewModel model = new TeacherSubjectViewModel();

            try
            {
                model = BaseViewModel.UniversalConvert<TeacherSubjectDTO, TeacherSubjectViewModel>(TeacherSubjectService.Get(id));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return model;
        }


        // GET: TeacherSubject/Details/5
        public ActionResult Details(string id)
        {
            return PartialView(GetTeacherSubject(id));
        }

        [HttpGet]
        // GET: TeacherSubject/Create
        public ActionResult Create(string idTeacher, string returnUrl)
        {
            //admin no
            try
            {                
                var teacherDTO = TeacherService.Get(idTeacher);
                var deanDTO = DeanService.Get(User.Identity.GetUserId());
                var facultyDTO = FacultyService.Get(deanDTO.FacultyId);

                if (facultyDTO.ListIdTeachers.Contains(idTeacher))
                {
                    var model = new AddSubjectToTeacherViewModel()
                    {
                        FacultyId = facultyDTO.Id,
                        TeacherId = idTeacher
                    };

                    model.Subjects = GetNonTeacherSubjects(idTeacher, facultyDTO.Id);

                    return View(model);
                }
                else
                {
                    TempData["errorMessage"] = "У вас не хватает прав на изменение";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return Redirect(returnUrl);
        }

        // POST: TeacherSubject/Create
        [HttpPost]
        public async Task<ActionResult> Create(AddSubjectToTeacherViewModel model, string returnUrl)
        {
            StringBuilder tempDataMessage = new StringBuilder();
            var tempDataErrorMessage = "";

            foreach (var item in model.SelectedIdSubjects)
            {
                var result = await TeacherSubjectService.AddAsync(new TeacherSubjectDTO()
                {
                    SubjectId = item,
                    TeacherId = model.TeacherId
                });
                string message = result.Message + "<br>";

                if (result.Succedeed)
                {
                    tempDataMessage.AppendLine(message);
                }
                else
                {
                    tempDataErrorMessage += message;
                    model.Subjects = GetNonTeacherSubjects(model.TeacherId, model.FacultyId);
                    return View(model);
                }
            }

            TempData["message"] = tempDataMessage;
            TempData["errorMessage"] = tempDataErrorMessage;

            return Redirect(returnUrl);
        }

        // GET: TeacherSubject/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeacherSubject/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        // GET: TeacherSubject/Delete/5
        public ActionResult Delete(string id = "")
        {
            return PartialView(GetTeacherSubject(id));
        }


        // POST: TeacherSubject/Delete/5
        [HttpPost]
        [Authorize(Roles ="dean, admin")]
        public ActionResult Delete(TeacherSubjectViewModel model, string returnUrl)
        {
            //admin no           
            bool checkRole = false;

            if (User.IsInRole("dean"))
            {
                if (IsCorrectDean(model.FacultyId))
                {
                    DeleteTeacherSubject(model);
                }
                else
                {
                    checkRole = true;
                }
            }

            if (checkRole)
            {
                TempData["errorMessage"] = "У вас не хватает прав на изменение";
            }

            return Redirect(returnUrl);
        }        

        private void DeleteTeacherSubject(TeacherSubjectViewModel model)
        {
            var result = TeacherSubjectService.FullRemove(model.Id);

            if (result.Succedeed)
            {
                var studentSubjects = StudentSubjectService.GetStudentSubjects(model.SubjectId, model.TeacherId);
                string resultMessage = result.Message + "<br>";

                TempData["message"] = resultMessage;

                foreach (var item in studentSubjects)
                {
                    result = StudentSubjectService.FullRemove(item.Id);

                    resultMessage = result.Message + "<br>";
                    if (result.Succedeed)
                    {
                        TempData["message"] += resultMessage;
                    }
                    else
                    {
                        TempData["errorMessage"] += resultMessage;
                    }
                }
            }
            else
            {
                TempData["errorMessage"] = result.Message;
            }
        }

        private Boolean IsCorrectDean(string idFaculty)
        {
            try
            {
                var deanDTO = DeanService.Get(User.Identity.GetUserId());
                return deanDTO.FacultyId == idFaculty;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return false;
            }            
        }


        private List<SubjectViewModel> GetNonTeacherSubjects(string idTeacher, string idFaculty)
        {
            return BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
             (SubjectService.GetSubjectsInFaculty(idFaculty)
             .Where(s => !s.ListIdTeachers.Contains(idTeacher))).ToList();
        }
    }
}