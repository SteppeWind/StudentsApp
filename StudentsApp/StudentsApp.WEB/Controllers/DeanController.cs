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
using Microsoft.AspNet.Identity;
using System.Text;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles = "dean, admin")]
    public class DeanController : Controller
    {
        private IDeanService DeanService;
        private IGroupService GroupService;
        private ITeacherService TeacherService;
        private IStudentService StudentService;
        private ISubjectService SubjectService;
        private IFacultyService FacultyService;

        public DeanController(
            IDeanService deanService,
            IGroupService groupService,
            IFacultyService facultyService,
            ITeacherService teacherService,
            IStudentService studentService,
            ISubjectService subjectService)
        {
            DeanService = deanService;
            GroupService = groupService;
            TeacherService = teacherService;
            StudentService = studentService;
            SubjectService = subjectService;
            FacultyService = facultyService;
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var deanVM = new ComplexDean();
            try
            {
                var deanDTO = DeanService.Get(id);

                deanVM = BaseViewModel.UniversalConvert<DeanDTO, ComplexDean>(deanDTO);

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
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View(deanVM);
        }

        private Boolean IsCorrectDean(string idFaculty)
        {
            var deanDTO = DeanService.Get(User.Identity.GetUserId());

            return deanDTO.FacultyId == idFaculty;
        }
    }
}