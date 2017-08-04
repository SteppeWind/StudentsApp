using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.ComplexEntities;
using StudentsApp.WEB.Models.Entities.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles = "dean, admin")]
    public class DeanController : Controller
    {
        private ITeacherService TeacherService;
        private IStudentService StudentService;
        private ISubjectService SubjectService;
        private IDeanService DeanService;
        private IGroupService GroupService;
        private IFacultyService FacultyService;

        public DeanController(
            IGroupService groupService,
            IFacultyService facultyService,
            IDeanService deanService,
            ITeacherService teacherService,
            IStudentService studentService,
            ISubjectService subjectService)
        {
            TeacherService = teacherService;
            StudentService = studentService;
            SubjectService = subjectService;
            DeanService = deanService;
            GroupService = groupService;
            FacultyService = facultyService;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                var deanDTO = DeanService.Get(id);

                var deanVM = BaseViewModel.UniversalConvert<DeanDTO, ComplexDean>(deanDTO);

                deanVM.Faculty = BaseViewModel.UniversalConvert<FacultyDTO, FacultyViewModel>
                    (FacultyService.Get(deanDTO.FacultyId));

                deanVM.Groups = BaseViewModel.UniversalConvert<GroupDTO, GroupViewModel>
                    (GroupService.GetGroupsInFaculty(deanDTO.FacultyId)).ToList();

                deanVM.Subjects = BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
                    (SubjectService.GetSubjectsInFaculty(deanDTO.FacultyId)).ToList();

                deanVM.StudentsMoreAverageMark = BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>
                    (StudentService.GetStudentsMoreMiddleAverageMark(deanDTO.FacultyId)).ToList();

                deanVM.TeachersWithMinCountStudents = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>
                    (TeacherService.GetTeachersWithMinCountStudents(deanDTO.FacultyId)).ToList();

                deanVM.TeachersWithAllStudents = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>
                    (TeacherService.GetTeachersWithAllStudents(deanDTO.FacultyId)).ToList();

                return View(deanVM);
            }
            catch (PersonNotFoundException ex)
            {

            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AddStudentToTeacher(int idTeacher = 3, string returnUrl = "")
        {
            var model = new AddStudentToTeacherViewModel();

            var deanDTO = await DeanService.GetByEmailAsync(User.Identity.Name);

            var teacherDTO = TeacherService.Get(idTeacher);

            if (teacherDTO.ListIdFaculties.Contains(deanDTO.FacultyId))
            {
                model.FacultyId = deanDTO.FacultyId;
                model.TeacherId = idTeacher;
                model.Subjects = GetTeacherSubjects(idTeacher, deanDTO.FacultyId);
                model.Students = GetAllStudents(deanDTO.FacultyId);

                var grouped = model.GropedStudents.ToList();

                return View(model);

            }
            else
            {
                TempData["errorMessage"] = "У вас не хватает прав на изменение";
                return Redirect(returnUrl);
            }
        }


        [HttpPost]
        public ActionResult AddStudentToTeacher(AddStudentToTeacherViewModel model, string returnUrl)
        {
            if (model.SelectedIdStudents.Any() && model.SelectedIdSubjects.Any())//if selected students ans subjects are not empty
            {
                foreach (var studentId in model.SelectedIdStudents)
                {
                    StudentService.AddSubject(Convert.ToInt32(studentId),
                        model.SelectedIdSubjects.Select(s => Convert.ToInt32(s)), model.TeacherId);
                }
                TempData["message"] = "Изменения были сохранены";
            }
            else
            {
                TempData["errorMessage"] = "Изменения не были сохранены";
                return Redirect(returnUrl);
            }

            model.Students = GetAllStudents(model.FacultyId);
            model.Subjects = GetTeacherSubjects(model.TeacherId, model.FacultyId);

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteStudentFromTeacher(int idFaculty, int idStudent, int idSubject, string returnUrl)
        {
            try
            {
                if (await IsCorrectDean(idFaculty))
                {
                    StudentService.RemoveSubject(idStudent, idSubject);
                    TempData["message"] = "Изменения были сохранены";
                }
                else
                {
                    TempData["errorMessage"] = "У вас не хватает прав на изменение";
                }
            }
            catch (ValidationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (PersonNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<ActionResult> AddSubjectToTeacher(int idTeacher, string returnUrl)
        {
            try
            {
                var teacherDTO = TeacherService.Get(idTeacher);
                var deanDTO = await DeanService.GetByEmailAsync(User.Identity.Name);
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

        [HttpPost]
        public ActionResult AddSubjectToTeacher(AddSubjectToTeacherViewModel model, string returnUrl)
        {
            try
            {
                foreach (var item in model.SelectedIdSubjects)
                {
                    TeacherService.AddSubject(int.Parse(item), model.TeacherId);
                }

                TempData["message"] = "Изменения были сохранены";
                return Redirect(returnUrl);
            }
            catch (ValidationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            model.Subjects = GetNonTeacherSubjects(model.TeacherId, model.FacultyId);

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> DeleteSubjectFromTeacher(int idFaculty, int idTeacher, int idSubject, string returnUrl)
        {
            try
            {
                if (await IsCorrectDean(idFaculty))
                {
                    TeacherService.RemoveSubject(idSubject, idTeacher);
                    var studentsDTO = StudentService.GetStudents(idFaculty);

                    foreach (var item in studentsDTO)
                    {
                        if (item.ListIdSubjects.Contains(idSubject))
                        {
                            StudentService.RemoveSubject(item.Id, idSubject);
                        }
                    }

                    TempData["message"] = "Изменения были сохранены";
                }
                else
                {
                    TempData["errorMessage"] = "У вас не хватает прав на изменение";
                }
            }
            catch (ValidationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return Redirect(returnUrl);
        }

        private async Task<Boolean> IsCorrectDean(int idFaculty)
        {
            var deanDTO = await DeanService.GetByEmailAsync(User.Identity.Name);

            return deanDTO.FacultyId == idFaculty;
        }

        //continue to apply mega shit
        private List<StudentViewModel> GetAllStudents(int idFaculty)
        {
            return BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>
                (StudentService.GetStudents(idFaculty)).ToList();
        }

        //continue to apply mega shit too
        private List<SubjectViewModel> GetTeacherSubjects(int idTeacher, int idFaculty)
        {
            return BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
              (SubjectService.GetSubjectsInFacultyFromTeacher(idFaculty, idTeacher)).ToList();
        }


        private List<SubjectViewModel> GetNonTeacherSubjects(int idTeacher, int idFaculty)
        {
            return BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
             (SubjectService.GetSubjectsInFaculty(idFaculty)
             .Where(s => !s.ListIdTeachers.Contains(idTeacher))).ToList();
        }
    }
}