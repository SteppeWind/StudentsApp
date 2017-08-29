using PagedList;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.ComplexEntities;
using StudentsApp.WEB.Models.Entities.Edit;
using StudentsApp.WEB.Models.Entities.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace StudentsApp.WEB.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        ITeacherService TeacherService;
        ISubjectService SubjectService;
        IStudentService StudentService;
        IDeanService DeanService;
        ITeacherPostService TeacherPostService;
        IFacultyService FacultyService;
        ITeacherFacultyService TeacherFacultyService;
        ITeacherSubjectService TeacherSubjectService;
        IStudentSubjectService StudentSubjectService;

        public TeacherController(
            ITeacherService teacherService,
            ISubjectService subjectService,
            IStudentService studentService,
            IDeanService deanService,
            ITeacherPostService teacherPostService,
            IFacultyService facultyService,
            IStudentSubjectService studentSubjectService,
            ITeacherSubjectService teacherSubjectService,
            ITeacherFacultyService teacherFacultyService)
        {
            SubjectService = subjectService;
            StudentService = studentService;
            TeacherService = teacherService;
            TeacherPostService = teacherPostService;
            FacultyService = facultyService;
            DeanService = deanService;
            TeacherFacultyService = teacherFacultyService;
            TeacherSubjectService = teacherSubjectService;
            StudentSubjectService = studentSubjectService;
        }

        int pageSize = 10;

        // GET: Teacher
        public ActionResult Index(int? page = 1)
        {
            int pageNumber = (page ?? 1);

            var teachersDTO = TeacherService.GetAll;

            var teachersVM = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>(teachersDTO).ToList();

            return View(teachersVM.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            ComplexTeacher teacherVM = new ComplexTeacher();

            try
            {
                var teacherDTO = TeacherService.Get(id);
                var postsDTO = TeacherFacultyService.GetTeacherPosts(id);
                var teacherSubjectsDTO = TeacherSubjectService.GetTeacherSubjects(id);
                var studentSubjectsDTO = StudentSubjectService.GetStudentSubjectsByTeacherId(id);

                teacherVM = BaseViewModel.UniversalConvert<TeacherDTO, ComplexTeacher>(teacherDTO);
                teacherVM.TeacherSubjects = BaseViewModel.UniversalConvert<TeacherSubjectDTO, TeacherSubjectViewModel>(teacherSubjectsDTO).ToList();
                teacherVM.Posts = BaseViewModel.UniversalConvert<TeacherFacultyDTO, TeacherFacultyVIewModel>(postsDTO).ToList();
                teacherVM.StudentsSubjects = BaseViewModel.UniversalConvert<StudentSubjectDTO, ComplexStudentSubject>(studentSubjectsDTO).ToList();

                foreach (var item in teacherVM.StudentsSubjects)
                {
                    item.Student = BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>(StudentService.Get(item.StudentId));
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            
            return View(teacherVM);
        }

        [HttpGet]
        [Authorize(Roles = "dean, admin")]
        public async Task<ActionResult> Create()
        {
            RegisterTeacher teacherVM = new RegisterTeacher();

            if (User.IsInRole("dean"))
            {
                var dean = await DeanService.GetByEmailAsync(User.Identity.Name);
                teacherVM.FacultyId = dean.FacultyId;
            }

            teacherVM.Subjects = GetSubjects(teacherVM.FacultyId);
            teacherVM.Posts = new SelectList(GetAllPosts, "Id", "NamePostTeacher");

            foreach (var item in teacherVM.GroupSubjects)
            {
                teacherVM.SelectedIdPosts.Add("");
            }

            return View(teacherVM);
        }

        [HttpPost]
        [Authorize(Roles = "dean, admin")]
        public async Task<ActionResult> Create(RegisterTeacher teacher)
        {
            if (ModelState.IsValid)
            {
                if (!teacher.SelectedIdSubjects.Any())
                {
                    TempData["errorMessage"] = "Вы не выбрали ни одного предмета";

                    FillCreateModel(teacher);
                    return View(teacher);
                }

                TeacherDTO teacherDTO = new TeacherDTO()
                {
                    Email = teacher.Email,
                    Name = teacher.Name,
                    Surname = teacher.Surname,
                    MiddleName = teacher.MiddleName,
                    Password = teacher.Password
                };
                var result = await TeacherService.AddAsync(teacherDTO);
                string message = result.Message + "<br>";

                if (result.Succedeed)
                {
                    TempData["message"] += message;

                    var results = await TeacherSubjectService
                        .AddByTeacherEmail(teacher.Email, teacher.SelectedIdSubjects
                        .Select(s => new TeacherSubjectDTO() { SubjectId = s }));
                    foreach (var item in results)
                    {
                        message = item.Message + "<br>";
                        if (item.Succedeed)
                        {
                            TempData["message"] += message;
                        }
                        else
                        {
                            TempData["errorMessage"] += message;
                        }
                    }

                    int count = FacultyService.Count;

                    for (int i = 0; i < count; i++)
                    {
                        var faculty = FacultyService.GetAll.ElementAt(i);
                        if (faculty.ListIdSubjects.Intersect(teacher.SelectedIdSubjects).Any())
                        {
                            string postIndex = "";
                            if (teacher.SelectedIdPosts.Count == 1)
                            {
                                postIndex = teacher.SelectedIdPosts.First();
                            }
                            else
                            {
                                postIndex = teacher.SelectedIdPosts[i];
                            }

                            result = await TeacherFacultyService.AddByTeacherEmail(teacher.Email, new TeacherFacultyDTO()
                            {
                                FacultyId = faculty.Id,
                                PostTeacherId = postIndex
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
                    }
                }
                else
                {
                    TempData["errorMessage"] += message;

                    FillCreateModel(teacher);
                    return View(teacher);
                }

                return Redirect($@"Details/{(await TeacherService.GetByEmailAsync(teacher.Email)).Id}");
            }

            FillCreateModel(teacher);
            return View(teacher);
        }


        private void FillCreateModel(RegisterTeacher teacher)
        {
            teacher.Subjects = GetSubjects(teacher.FacultyId);
            teacher.Posts = new SelectList(GetAllPosts, "Id", "NamePostTeacher");
        }

        [HttpGet]
        public ActionResult AddSubjectToStudent(string idStudent, string returnUrl)
        {
            var model = new AddSubjectToStudentViewModel();
            try
            {
                var teacherDTO = TeacherService.Get(User.Identity.GetUserId());
                var studentDTO = StudentService.Get(idStudent);
                var facultiesDTO = FacultyService.GetAll.Where(f => teacherDTO.ListIdFaculties.Contains(f.Id));

                model.StudentId = idStudent;
                bool check = false;

                foreach (var item in facultiesDTO)
                {
                    if (item.ListIdGroups.Intersect(studentDTO.ListIdGroups).Any())
                    {
                        check = true;
                        break;
                    }
                }

                if (check)
                {
                    IEnumerable<string> listSubjects = teacherDTO.ListIdSubjects;
                    var subjectsVM = GetTeacherSubjects(teacherDTO.Id);
                    model.ListSubjects = new SelectList(subjectsVM, "Id", "SubjectName");
                    model.TeacherId = teacherDTO.Id;
                }
                else
                {
                    TempData["errorMessage"] = "У вас не хватает прав";
                    return Redirect(returnUrl);
                }
            }
            catch (PersonNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(returnUrl);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddSubjectToStudent(AddSubjectToStudentViewModel model, string returnUrl)
        {
            string message = string.Empty;

            foreach (var item in model.ListIdSubjects)
            {
                var result = await StudentSubjectService.AddAsync(new StudentSubjectDTO()
                {
                    StudentId = model.StudentId,
                    SubjectId = item,
                    TeacherId = model.TeacherId
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

            model.ListSubjects = new SelectList(GetTeacherSubjects(model.TeacherId), "Id", "SubjectName");
            return Redirect(returnUrl);
        }

        public List<SubjectViewModel> GetTeacherSubjects(string teacherId)
        {
            return BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
                (SubjectService.GetAll.Where(s => s.ListIdTeachers.Contains(teacherId))).ToList();
        }

        public List<SubjectViewModel> GetSubjects(string idFaculty) => GetAllSubjects.Where(s => s.FacultyId == idFaculty).ToList();

        public List<SubjectViewModel> GetAllSubjects =>
             BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>(SubjectService.GetAll).ToList();

        public List<TeacherPostViewModel> GetAllPosts =>
             BaseViewModel.UniversalConvert<PostTeacherDTO, TeacherPostViewModel>(TeacherPostService.GetAll).ToList();

    }
}