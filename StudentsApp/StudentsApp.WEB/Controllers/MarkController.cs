using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.ComplexEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles = "teacher, dean, admin")]
    public class MarkController : Controller
    {
        ITeacherService TeacherService;
        ISubjectService SubjectService;
        IStudentService StudentService;
        IMarkService MarkService;
        IDeanService DeanService;
        IFacultyService FacultyService;

        public MarkController(
            IMarkService markService,
            ITeacherService teacherService,
            ISubjectService subjectService,
            IStudentService studentService,
            IDeanService deanService,
            IFacultyService facultyService)
        {
            TeacherService = teacherService;
            SubjectService = subjectService;
            StudentService = studentService;
            MarkService = markService;
            DeanService = deanService;
            FacultyService = facultyService;
        }


        [HttpGet]
        public ActionResult Create(string idStudent, SubjectTypeViewModel type, string returnUrl)
        {
            ComplexMark mark = new ComplexMark();
            mark.Type = type;
            try
            {
                var teacherDTO = TeacherService.Get(User.Identity.GetUserId());
                if (idStudent != null)
                {
                    if (!teacherDTO.ListIdStudents.Contains(idStudent))
                    {
                        TempData["errorMessage"] = "У вас не хватает прав на изменение";
                        return Redirect(returnUrl);
                    }
                    mark.StudentId = idStudent;
                    ViewBag.StudentName = StudentService.Get(idStudent).FullName;

                    mark.TeacherId = teacherDTO.Id;
                    GetInfo(mark, teacherDTO.Id, mark.StudentId);
                }
            }
            catch (PersonNotFoundException ex)
            {
                //GetInfo(mark, 0);
                TempData["errorMessage"] = ex.Message;
                return Redirect(returnUrl);
            }

            return View(mark);
        }


        [HttpPost]
        public async Task<ActionResult> Create(ComplexMark mark, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MarkDTO markDTO = new MarkDTO();
                //sorry, it`s very bad, but i dont know how to do differently
                if (mark.Type == SubjectTypeViewModel.Exam)
                {
                    markDTO = BaseViewModel.UniversalReverseConvert<ComplexMark, ExamMarkDTO>(mark);
                    (markDTO as ExamMarkDTO).Mark = byte.Parse(mark.SelectedVariant);
                }
                else
                {
                    markDTO = BaseViewModel.UniversalReverseConvert<ComplexMark, TestMarkDTO>(mark);
                    (markDTO as TestMarkDTO).IsPassed = mark.SelectedVariant == "Зачет" ? true : false;
                }

                var result = await MarkService.AddAsync(markDTO);
                if (result.Succedeed)
                {
                    TempData["message"] = result.Message;
                }
                else
                {
                    TempData["errorMessage"] = result.Message;
                }

                return Redirect(returnUrl);
            }

            GetInfo(mark, mark.TeacherId, mark.StudentId);
            return View(mark);
        }


        private TMark GetInfo<TMark>(TMark markVM, string teacherId, string studentId) where TMark : ComplexMark
        {
            List<SubjectViewModel> subjectsVM;

            if (string.IsNullOrEmpty(teacherId))
            {

                var studentsVM = BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>(StudentService.GetAll).ToList();
                var teachersVM = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>(TeacherService.GetAll).ToList();

                markVM.ListStudents = new SelectList(studentsVM, "Id", "FullName", markVM.StudentId);
                markVM.ListTeachers = new SelectList(teachersVM, "Id", "FullName", markVM.TeacherId);

                subjectsVM = BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
                   (SubjectService.GetAll).ToList();
            }
            else
            {
                subjectsVM = BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
                    (SubjectService.GetAll.Where(s => s.ListIdTeachers.Contains(teacherId) && s.ListIdStudents.Contains(studentId))).ToList();
            }

            markVM.ListSubjects = new SelectList(subjectsVM, "Id", "SubjectName", markVM.SubjectId);
            FillOptionsMark(markVM);

            return markVM;
        }


        [HttpGet]
        public ActionResult Edit(string id, string teacherId, string returnUrl = "")
        {
            ComplexMark markVM = new ComplexMark();
            try
            {
                var markDTO = MarkService.Get(id);
                if (IsContainsSubject(markDTO.SubjectId) && User.Identity.GetUserId() == teacherId)
                {
                    markVM = BaseViewModel.UniversalConvert<MarkDTO, ComplexMark>(markDTO);
                    string selectedValue = null;

                    if (markDTO is ExamMarkDTO examMarkDTO)
                    {
                        selectedValue = examMarkDTO.Mark.ToString();
                    }
                    else
                    {
                        selectedValue = (markDTO as TestMarkDTO).IsPassed ? "Зачет" : "Незачет";
                    }

                    FillOptionsMark(markVM, selectedValue);
                }
                else
                {
                    TempData["errorMessage"] = "У вас не хватает прав на изменение";
                    return Redirect(returnUrl);
                }
            }
            catch (ValidationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(returnUrl);
            }

            return View(markVM);
        }

        private bool IsContainsSubject(string idSubject)
        {
            try
            {
                var teacher = TeacherService.Get(User.Identity.GetUserId());

                return teacher.ListIdSubjects.Contains(idSubject);
            }
            catch (PersonNotFoundException ex)
            {
                var dean = DeanService.Get(User.Identity.GetUserId());                
                var check = FacultyService.IsHaveSubjectFromFaculty(idSubject, dean.FacultyId);

                if (!check)
                {
                    TempData["errorMessage"] = ex.Message;
                }

                return check;
            }
        }

        //filling list for choose mark
        private void FillOptionsMark(ComplexMark markVM, string selectedValue = null)
        {
            if (markVM.Type == SubjectTypeViewModel.Exam)
            {
                markVM.Options = new SelectList(new int[] { 2, 3, 4, 5 });
            }
            else
            {
                markVM.Options = new SelectList(new string[] { "Зачет", "Незачет" });
            }

            markVM.SelectedVariant = selectedValue;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ComplexMark markVM, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MarkDTO markDTO = new MarkDTO();
                if (markVM.Type == SubjectTypeViewModel.Exam)
                {
                    markDTO = BaseViewModel.UniversalReverseConvert<ComplexMark, ExamMarkDTO>(markVM);
                    (markDTO as ExamMarkDTO).Mark = byte.Parse(markVM.SelectedVariant);
                }
                else
                {
                    markDTO = BaseViewModel.UniversalReverseConvert<ComplexMark, TestMarkDTO>(markVM);
                    (markDTO as TestMarkDTO).IsPassed = markVM.SelectedVariant == "Зачет" ? true : false;
                }

                var result =  await MarkService.UpdateAsync(markDTO);
                if (result.Succedeed)
                {
                    TempData["message"] = result.Message;
                }
                else
                {
                    TempData["errorMessage"] = result.Message;
                }

                return Redirect(returnUrl);
            }

            FillOptionsMark(markVM);

            return View(markVM);
        }


        [HttpPost]
        public ActionResult Delete(string id, string teacherId, string returnUrl)
        {
            var markDTO = MarkService.Get(id);
            if (IsContainsSubject(markDTO.SubjectId) && User.Identity.GetUserId() == teacherId)
            {
                var result = MarkService.FullRemove(id);

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

            return Redirect(returnUrl);
        }

        // GET: Mark
        public ActionResult Index()
        {
            return View();
        }
    }
}