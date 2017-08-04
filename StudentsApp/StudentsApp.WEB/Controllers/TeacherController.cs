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

namespace StudentsApp.WEB.Controllers
{
    public class TeacherController : Controller
    {
        ITeacherService TeacherService;
        ISubjectService SubjectService;
        IStudentService StudentService;
        IDeanService DeanService;
        ITeacherPostService TeacherPostService;
        IFacultyService FacultyService;

        public TeacherController(
            ITeacherService teacherService,
            ISubjectService subjectService,
            IStudentService studentService,
            IDeanService deanService,
            ITeacherPostService teacherPostService,
            IFacultyService facultyService)
        {
            SubjectService = subjectService;
            StudentService = studentService;
            TeacherService = teacherService;
            TeacherPostService = teacherPostService;
            FacultyService = facultyService;
            DeanService = deanService;
        }

        int pageSize = 10;

        // GET: Teacher
        public ActionResult Index(int? page = 1)
        {
            int pageNumber = (page ?? 1);

            var teachersDTO = TeacherService.GetAll.ToList();

            var teachersVM = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>(teachersDTO).ToList();

            return View(teachersVM.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int id = 3)
        {
            ComplexTeacher teacherVM = new ComplexTeacher();

            var teacherDTO = TeacherService.Get(id);
            var postsDTO = TeacherService.GetPosts(id);
            var subjectsDTO = SubjectService.GetAll.Where(s => s.ListIdTeachers.Contains(id)).ToList();

            teacherVM = BaseViewModel.UniversalConvert<TeacherDTO, ComplexTeacher>(teacherDTO);
            teacherVM.Subjects = BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>(subjectsDTO).ToList();
            teacherVM.Posts = BaseViewModel.UniversalConvert<TeacherFacultyDTO, TeacherFacultyVIewModel>(postsDTO).ToList();
           
            foreach (var item in subjectsDTO)
            {
                var ss = BaseViewModel.UniversalConvert<SubjectDTO, SubjectWithStudents>(item);
                //search students whose subjects id`s intersect with teacher of subjects id`s 
                var studentsDTO = StudentService.GetStudents(item.Id, teacherDTO.Id);
               
                ss.Students.AddRange(BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>(studentsDTO));
                teacherVM.StudentsSubjects.Add(ss);
            }


            return View(teacherVM);
        }


        [HttpGet]
        [Authorize]
        private async Task<ActionResult> Details(string email)
        {
            var teacher = await TeacherService.GetByEmailAsync(email);
            return View("Details", teacher.Id);
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
                teacherVM.SelectedIdPosts.Add(0);
            }

            return View(teacherVM);
        }

        [HttpPost]
        [Authorize(Roles = "dean, admin")]
        public ActionResult Create(RegisterTeacher teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TeacherDTO teacherDTO = new TeacherDTO()
                    {
                        Email = teacher.Email,
                        Name = teacher.Name,
                        Surname = teacher.Surname,
                        MiddleName = teacher.MiddleName,
                        Password = teacher.Password
                    };
                    TeacherService.Add(teacherDTO);

                    var subjectsId = teacher.SelectedIdSubjects.Select(s => Convert.ToInt32(s)).ToList();

                    TeacherService.AddSubject(subjectsId, teacher.Email);

                    for (int i = 0; i < FacultyService.GetAll.Count(); i++)
                    {
                        var faculty = FacultyService.GetAll.ElementAt(i);
                        if (faculty.ListIdSubjects.Intersect(subjectsId).Any())
                        {
                            TeacherService.AddTeacherPost(teacher.Email, faculty.Id, teacher.SelectedIdPosts.First());
                        }
                    }

                    return Redirect($@"Details/{TeacherService.GetAll.Last().Id}");
                }
                catch (PersonIsExistException ex)
                {
                    TempData["message"] = ex.Message;
                }
                catch (ValidationException ex)
                {
                    TempData["message"] = ex.Message;
                }

            }

            teacher.Subjects = GetSubjects(teacher.FacultyId);
            teacher.Posts = new SelectList(GetAllPosts, "Id", "NamePostTeacher");

            return View(teacher);
        }


        [HttpGet]
        public ActionResult AddSubjectToStudent(int idStudent, string returnUrl)
        {
            var model = new AddSubjectToStudentViewModel() { StudentId = idStudent };
            try
            {
                var teacherDTO = TeacherService.GetByEmailAsync(User.Identity.Name).Result;
                var studentDTO = StudentService.Get(idStudent);
                var facultiesDTO = FacultyService.GetAll.Where(f => teacherDTO.ListIdFaculties.Contains(f.Id));

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
                    IEnumerable<int> listSubjects = StudentService.Get(idStudent).ListIdSubjects;
                    var subjectsVM = GetAllSubjects.Where(s => teacherDTO.ListIdSubjects.Except(listSubjects).Contains(s.Id));
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
        public ActionResult AddSubjectToStudent(AddSubjectToStudentViewModel model, string returnUrl)
        {
            if (model.ListIdSubjects.Count == 0)
            {
                TempData["errorMessage"] = "Вы не выбрали ни одного предмета";
            }
            else
            {
                try
                {
                    foreach (var item in model.ListIdSubjects)
                    {
                        StudentService.AddSubject(model.StudentId, int.Parse(item), model.TeacherId);
                    }

                    TempData["message"] = "Изменения были сохранены";
                    return Redirect(returnUrl);
                }
                catch (ValidationException ex)
                {
                    TempData["errorMessage"] = ex.Message;
                }
                catch(PersonNotFoundException ex)
                {
                    TempData["errorMessage"] = ex.Message;
                }
            }

            model.ListSubjects = new SelectList(GetTeacherSubjects(model.TeacherId), "Id", "SubjectName");
            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveSubjectFromStudent(int idStudent, int idSubject, string returnUrl)
        {           
            try
            {
                var teacherDTO = await TeacherService.GetByEmailAsync(User.Identity.Name);

                if (teacherDTO.ListIdSubjects.Contains(idSubject))
                {
                    StudentService.RemoveSubject(idStudent, idSubject);
                    TempData["message"] = "Изменения были сохранены";
                }
                else
                {
                    TempData["errorMessage"] = "У вас нет прав редактировать данный предмет";
                    return Redirect(returnUrl);
                }
            }
            catch (PersonNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (ValidationException ex)
            {
                TempData["errorMessage"] = "Изменения не сохранены";
            }
            
            return Redirect(returnUrl);
        }

        public List<SubjectViewModel> GetTeacherSubjects(int teacherId)
        {
            return BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>
                (SubjectService.GetAll.Where(s => s.ListIdTeachers.Contains(teacherId))).ToList();
        }

        public List<SubjectViewModel> GetSubjects(int idFaculty) => GetAllSubjects.Where(s => s.FacultyId == idFaculty).ToList();

        public List<SubjectViewModel> GetAllSubjects =>
             BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>(SubjectService.GetAll).ToList();

        public List<TeacherPostViewModel> GetAllPosts =>
             BaseViewModel.UniversalConvert<PostTeacherDTO, TeacherPostViewModel>(TeacherPostService.GetAll).ToList();

    }
}