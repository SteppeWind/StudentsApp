using PagedList;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.ComplexEntities;
using StudentsApp.WEB.Models.Entities.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    public class StudentController : Controller
    {
        private IStudentService StudentService;
        private IGroupService GroupService;
        private IMarkService MarkService;
        private IFacultyService FacultyService;
        private ISubjectService SubjectService;
        private ITeacherService TeacherService;
        private IDeanService DeanService;

        public StudentController(
            IStudentService studentService,
            IGroupService groupService,
            IDeanService deanService,
            IMarkService markService,
            IFacultyService facultyService,
            ISubjectService subjectService,
            ITeacherService teacherService)
        {
            StudentService = studentService;
            GroupService = groupService;
            MarkService = markService;
            FacultyService = facultyService;
            SubjectService = subjectService;
            TeacherService = teacherService;
            DeanService = deanService;
        }


        int pageSize = 10;

        [HttpGet]
        public ActionResult Index(int? page = 1)
        {
            int pageNumber = (page ?? 1);
            return View(BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>(StudentService.GetAll).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int id = 21)
        {
            ComplexStudent student = new ComplexStudent();
            try
            {
                var studentDTO = StudentService.Get(id);
                var studentGroupsDTO = GroupService.GetStudentGroups(id);
                var studentMarksDTO = MarkService.GetStudentMarks(id);
                var studentSubjectsDTO = SubjectService.GetStudentSubjects(id);

                student = BaseViewModel.UniversalConvert<StudentDTO, ComplexStudent>(studentDTO);
                student.Groups = BaseViewModel.UniversalConvert<GroupDTO, GroupViewModel>(studentGroupsDTO).ToList();
                student.Subjects = BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>(studentSubjectsDTO).ToList();
               
                foreach (var mark in studentMarksDTO)
                {
                    MarkViewModel markVM = new MarkViewModel();
                    if (mark.Type == SubjectTypeDTO.Exam)
                    {
                        markVM = BaseViewModel.UniversalConvert<ExamMarkDTO, ExamMarkViewModel>(mark as ExamMarkDTO);
                    }
                    else
                    {
                        markVM = BaseViewModel.UniversalConvert<TestMarkDTO, TestMarkViewModel>(mark as TestMarkDTO);
                    }

                    markVM.Teacher = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>(TeacherService.Get(mark.TeacherId));
                    student.Marks.Add(markVM);
                }
            }
            catch (Exception ex)
            {
                
            }


            return View(student);
        }


        [HttpGet]
        [Authorize(Roles = "teacher, dean, admin")]
        public ActionResult Edit(int idStudent = 21)
        {
            var studentVM = BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>(StudentService.Get(idStudent));
            return View(studentVM);
        }

        [HttpPost]
        [Authorize(Roles = "teacher, dean, admin")]
        public ActionResult Edit(StudentViewModel student)//update student
        {
            if (ModelState.IsValid)
            {
                var studentDTO = BaseViewModel.UniversalReverseConvert<StudentViewModel, StudentDTO>(student);
                StudentService.Update(studentDTO);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "teacher, dean, admin")]
        public async Task<ActionResult> Create()
        {
            int idFaculty = 1;
            
            if (User.IsInRole("teacher"))
            {
                var teacher = await TeacherService.GetByEmailAsync(User.Identity.Name);
                idFaculty = FacultyService.GetAll.FirstOrDefault(f => f.ListIdTeachers.Contains(teacher.Id)).Id;
            }

            if (User.IsInRole("dean"))
            {
                var dean = await DeanService.GetByEmailAsync(User.Identity.Name);
                idFaculty = dean.FacultyId;
            }


            var registerStudent = new RegisterStudent() { FacultyId = idFaculty };

            //StudentService.FullRemove(21);

            registerStudent.Subjects = GetAllSubjects(idFaculty);
            registerStudent.Groups = GetAllGroups(idFaculty);

            registerStudent.SubjectsWithTeachers = GetAllSubjectsWithTeachers(idFaculty);

            return View(registerStudent);
        }

        [HttpPost]
        [Authorize(Roles = "teacher, dean, admin")]
        public ActionResult Create(RegisterStudent student)//registration student
        {
            student.SubjectsWithTeachers = GetAllSubjectsWithTeachers(student.FacultyId).ToList();
            var selectedIdTeach = new List<int>();

            var subjects = SubjectService.GetAll.Where(s => s.FacultyId == student.FacultyId).Select(s => s.Id).ToList();
            int length = subjects.Count;
            //ищем разность всех айдишников предметов с выбранными, нужно, что восстановить пробелы между преподавателем и его дисциплиной
            var exceptIdSubjects = subjects.Except(student.SelectedIdSubjects.Select(s => Convert.ToInt32(s)).ToList());


            for (int i = 0; i < length; i++)
            {
                //если в разности нет предмета, то мы нашли чекнутого преподавателя в соответствующем списке
                if (!exceptIdSubjects.Contains(subjects[i]))
                {
                    selectedIdTeach.Add(student.SelectedIdTeachers[i]);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    StudentDTO studentDTO = new StudentDTO()
                    {
                        Name = student.Name,
                        Surname = student.Surname,
                        MiddleName = student.MiddleName,
                        Email = student.Email,
                        Password = student.Password
                    };
                    StudentService.Add(studentDTO);

                    GroupService.AddStudentToGroup(student.IdGroup, student.Email);

                    StudentService.AddSubject(student.Email, student.SelectedIdSubjects.Select(s => Convert.ToInt32(s)).ToList(), selectedIdTeach);

                    return Redirect($@"Details/{StudentService.GetAll.Last().Id}");
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

            //apply mega shit
            student.Groups = GetAllGroups(student.FacultyId);
            student.Subjects = GetAllSubjects(student.FacultyId);

            return View(student);
        }


        private List<SubjectWithTeachers> GetAllSubjectsWithTeachers(int idFaculty)
        {
            return GetAllSubjectsWithTeachers(SubjectService.GetSubjectsInFaculty(idFaculty).Select(s => s.Id));
        }

        private List<SubjectWithTeachers> GetAllSubjectsWithTeachers(IEnumerable<int> subjectsId)
        {
            List<SubjectWithTeachers> result = new List<SubjectWithTeachers>();
            foreach (var item in subjectsId)
            {
                var subject = BaseViewModel.UniversalConvert<SubjectDTO, SubjectWithTeachers>(SubjectService.Get(item));
                var teachers = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>(TeacherService.GetAll.Where(t => t.ListIdSubjects.Contains(item)));
                subject.Teachers = teachers.ToList();

                result.Add(subject);
            }

            return result;
        }

        //mega shit!!!!
        private List<SubjectViewModel> GetAllSubjects(int idFaculty) =>
            BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>(SubjectService.GetSubjectsInFaculty(idFaculty)).ToList();

        //mega shit too!!!!
        private List<GroupViewModel> GetAllGroups(int idFaculty) =>
            BaseViewModel.UniversalConvert<GroupDTO, GroupViewModel>(GroupService.GetGroupsInFaculty(idFaculty)).ToList();

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
    }
}