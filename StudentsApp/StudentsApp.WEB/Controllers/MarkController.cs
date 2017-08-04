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
        public async Task<ActionResult> Create(int? idStudent, SubjectTypeViewModel type, string returnUrl)
        {
            ComplexMark mark = new ComplexMark();
            mark.Type = type;
            try
            {
                var teacherDTO = await TeacherService.GetByEmailAsync(User.Identity.Name);
                if (idStudent != null)
                {
                    if (!teacherDTO.ListIdStudents.Contains((int)idStudent))
                    {
                        TempData["errorMessage"] = "У вас не хватает прав на изменение";
                        return Redirect(returnUrl);
                    }
                    mark.StudentId = (int)idStudent;
                    ViewBag.StudentName = StudentService.Get((int)idStudent).FullName;

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
        public ActionResult Create(ComplexMark mark, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
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

                    MarkService.Add(markDTO);
                    TempData["message"] = "Изменения были сохранены";

                    return Redirect(returnUrl);
                }
                catch (ValidationException)
                {

                }
            }

            GetInfo(mark, mark.TeacherId, mark.StudentId);
            return View(mark);
        }


        private TMark GetInfo<TMark>(TMark markVM, int teacherId, int studentId) where TMark : ComplexMark
        {
            List<SubjectViewModel> subjectsVM;

            if (teacherId == 0)
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
        public async Task<ActionResult> Edit(int id = 1, string returnUrl = "")
        {
            ComplexMark markVM = new ComplexMark();
            try
            {
                var markDTO = MarkService.Get(id);
                if (await IsContainsSubject(markDTO.SubjectId))
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

        private async Task<bool> IsContainsSubject(int idSubject)
        {
            try
            {
                var teacher = await TeacherService.GetByEmailAsync(User.Identity.Name);

                return teacher.ListIdSubjects.Contains(idSubject);
            }
            catch (PersonNotFoundException ex)
            {
                var dean = await DeanService.GetByEmailAsync(User.Identity.Name);
                var faculty = FacultyService.GetAll.FirstOrDefault(f => f.ListIdSubjects.Contains(idSubject) && dean.FacultyId == f.Id);

                if (faculty != null)
                    return true;

                TempData["errorMessage"] = ex.Message;
            }

            return false;
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
        public ActionResult Edit(ComplexMark markVM, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
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

                    MarkService.Update(markDTO);

                    TempData["message"] = "Изменения были сохранены";
                    return Redirect(returnUrl);
                }
                catch (ValidationException ex)
                {

                }
            }

            FillOptionsMark(markVM);

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int id, string returnUrl)
        {
            try
            {
                var markDTO = MarkService.Get(id);
                if (await IsContainsSubject(markDTO.SubjectId))
                {
                    MarkService.FullRemove(id);
                    TempData["message"] = "Изменения были сохранены";
                }
                else
                {
                    TempData["errorMessage"] = "У вас не хватает прав на изменение";
                }
            }
            catch (PersonNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (ValidationException ex)
            {
                TempData["errorMessage"] = ex.Message;
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