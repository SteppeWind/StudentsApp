using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StudentsApp.WEB.Models.Entities.Edit;
using StudentsApp.BLL.DTO;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.BLL.Infrastructure;
using System.Threading.Tasks;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles = "teacher, dean, admin")]
    public class StudentSubjectController : Controller
    {
        private IDeanService DeanService;
        private ITeacherService TeacherService;
        private IStudentService StudentService;
        private ISubjectService SubjectService;
        private IStudentSubjectService StudentSubjectService;

        public StudentSubjectController(
            IDeanService deanService,
            ITeacherService teacherService,
            IStudentService studentService,
            ISubjectService subjectService,
            IStudentSubjectService studentSubjectService)
        {
            DeanService = deanService;
            TeacherService = teacherService;
            StudentService = studentService;
            SubjectService = subjectService;
            StudentSubjectService = studentSubjectService;
        }

        private StudentSubjectViewModel GetStudentSubject(string id)
        {
            var result = new StudentSubjectDTO();

            try
            {
                result = StudentSubjectService.Get(id);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return BaseViewModel.UniversalConvert<StudentSubjectDTO, StudentSubjectViewModel>(result);
        }

        // GET: StudentSubject
        public ActionResult Index()
        {
            return View();
        }

        
        // GET: StudentSubject/Details/5
        public ActionResult Details(string id)
        {
            return PartialView(GetStudentSubject(id));
        }

        // GET: StudentSubject/Create       
        public ActionResult Create(string idTeacher, string returnUrl)
        {
            var model = new CreateStudentSubject();

            var deanDTO = DeanService.Get(User.Identity.GetUserId());

            var teacherDTO = TeacherService.Get(idTeacher);

            if (teacherDTO.ListIdFaculties.Contains(deanDTO.FacultyId))
            {
                model.FacultyId = deanDTO.FacultyId;
                model.TeacherId = idTeacher;
                model.Subjects = GetTeacherSubjects(idTeacher, deanDTO.FacultyId);
                model.Students = GetAllStudents(deanDTO.FacultyId);

                return View(model);
            }
            else
            {
                TempData["errorMessage"] = "У вас не хватает прав на изменение";
                return Redirect(returnUrl);
            }
        }

        // POST: StudentSubject/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateStudentSubject model, string returnUrl)
        {            
            //only dean actions

            foreach (var studentId in model.SelectedIdStudents)
            {
                foreach (var subjectId in model.SelectedIdSubjects)
                {
                    var result = await StudentSubjectService.AddAsync(new StudentSubjectDTO()
                    {
                        StudentId = studentId,
                        SubjectId = subjectId,
                        TeacherId = model.TeacherId
                    });
                    string message = result.Message + "<br>";

                    if (result.Succedeed)
                    {
                        TempData["message"] += message;
                    }
                    else
                    {
                        TempData["errorMessage"] += message;
                        model.Students = GetAllStudents(model.FacultyId);
                        model.Subjects = GetTeacherSubjects(model.TeacherId, model.FacultyId);
                        return View(model);
                    }
                }
            }

            return Redirect(returnUrl);
        }

        // GET: StudentSubject/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentSubject/Edit/5
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

        // GET: StudentSubject/Delete/5
        public ActionResult Delete(string id = "")
        {
            return PartialView(GetStudentSubject(id));
        }

        // POST: StudentSubject/Delete/5
        [HttpPost]
        public ActionResult Delete(StudentSubjectViewModel model, string returnUrl)
        {
            bool checkRole = false;
            if (User.IsInRole("teacher"))
            {
                if (model.TeacherId == User.Identity.GetUserId())
                {
                    DeleteStudentSubject(model.Id);
                }
                else
                {
                    checkRole = true;
                }
            }

            if (User.IsInRole("dean"))
            {
                if (IsCorrectDean(model.FacultyId))
                {
                    DeleteStudentSubject(model.Id);
                }
                else
                {
                    checkRole = true;
                }
            }

            if (User.IsInRole("admin"))
            {

            }
               
            if (checkRole)
            {
                TempData["errorMessage"] = "У вас не хватает прав на изменение";
            }

            return Redirect(returnUrl);
        }

        private void DeleteStudentSubject(string id)
        {
            var result = StudentSubjectService.FullRemove(id);

            if (result.Succedeed)
            {
                TempData["message"] = result.Message;
            }
            else
            {
                TempData["errorMessage"] = result.Message;
            }
        } 

        private void DeleteIfCurrentUserIsDean(DeanDTO dean, StudentSubjectViewModel model)
        {
            if (IsCorrectDean(model.FacultyId))
            {
                var result = StudentSubjectService.FullRemove(model.Id);

                if (result.Succedeed)
                {
                    TempData["message"] = result.Message;
                }
                else
                {
                    TempData["errorMessage"] = result.Message;
                }
            }
            else
            {
                TempData["errorMessage"] = "У вас не хватает прав на изменение";
            }
        }


        private Boolean IsCorrectDean(string idFaculty)
        {
            var deanDTO = DeanService.Get(User.Identity.GetUserId());

            return deanDTO.FacultyId == idFaculty;
        }

        //continue to apply mega shit
        private List<StudentViewModel> GetAllStudents(string idFaculty)
        {
            return BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>
                (StudentService.GetStudents(idFaculty)).ToList();
        }

        //continue to apply mega shit too
        private List<SubjectViewModel> GetTeacherSubjects(string idTeacher, string idFaculty)
        {
            return BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
              (SubjectService.GetSubjectsInFacultyFromTeacher(idFaculty, idTeacher)).ToList();
        }
    }
}