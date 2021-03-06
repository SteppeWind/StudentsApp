﻿using PagedList;
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
    [Authorize]
    public class StudentController : Controller
    {
        private IStudentService StudentService;
        private IGroupService GroupService;
        private IMarkService MarkService;
        private IFacultyService FacultyService;
        private ISubjectService SubjectService;
        private ITeacherService TeacherService;
        private IDeanService DeanService;
        private IStudentSubjectService StudentSubjectService;

        public StudentController(
            IStudentService studentService,
            IGroupService groupService,
            IDeanService deanService,
            IMarkService markService,
            IFacultyService facultyService,
            ISubjectService subjectService,
            ITeacherService teacherService,
            IStudentSubjectService studentSubjectService)
        {
            StudentService = studentService;
            GroupService = groupService;
            MarkService = markService;
            FacultyService = facultyService;
            SubjectService = subjectService;
            TeacherService = teacherService;
            DeanService = deanService;
            StudentSubjectService = studentSubjectService;
        }        

        int pageSize = 10;

        [HttpGet]
        public ActionResult Index(int? page = 1)
        {
            int pageNumber = (page ?? 1);
            return View(BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>(StudentService.GetAll).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            ComplexStudent student = new ComplexStudent();
            try
            {
                var studentDTO = StudentService.Get(id);
                var studentGroupsDTO = GroupService.GetStudentGroups(id);
                var studentMarksDTO = MarkService.GetStudentMarks(id);
                var studentSubjectsDTO = StudentSubjectService.GetStudentSubjectsByStudentId(id).ToList();

                student = BaseViewModel.UniversalConvert<StudentDTO, ComplexStudent>(studentDTO);
                student.Groups = BaseViewModel.UniversalConvert<GroupDTO, GroupViewModel>(studentGroupsDTO).ToList();
                student.StudentSubjects = BaseViewModel.UniversalConvert<StudentSubjectDTO, StudentSubjectViewModel>(studentSubjectsDTO).ToList();

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
                TempData["errorMessage"] = ex.Message;
            }

            return View(student);
        }


        //[HttpGet]
        //[Authorize(Roles = "teacher, dean, admin")]
        //public ActionResult Edit(string idStudent)
        //{
        //    var studentVM = BaseViewModel.UniversalConvert<StudentDTO, StudentViewModel>(StudentService.Get(idStudent));
        //    return View(studentVM);
        //}

        //[HttpPost]
        //[Authorize(Roles = "teacher, dean, admin")]
        //public ActionResult Edit(StudentViewModel student)//update student
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var studentDTO = BaseViewModel.UniversalReverseConvert<StudentViewModel, StudentDTO>(student);
        //        var result = StudentService.Update(studentDTO);
        //        if (result.Succedeed)
        //        {

        //        }
        //    }
        //    return View();
        //}

        [HttpGet]
        [Authorize(Roles = "teacher, dean, admin")]
        public async Task<ActionResult> Create()
        {
            string idFaculty = string.Empty;

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
        public async Task<ActionResult> Create(RegisterStudent student)//registration student
        {
            var selectedIdTeach = new List<string>();

            var subjects = SubjectService.GetSubjectsInFaculty(student.FacultyId).Select(s => s.Id).ToList();
            int length = subjects.Count;
            //ищем разность всех айдишников предметов с выбранными, нужно, что восстановить пробелы между преподавателем и его дисциплиной
            var exceptIdSubjects = subjects.Except(student.SelectedIdSubjects).ToList();

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
                StudentDTO studentDTO = new StudentDTO()
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    MiddleName = student.MiddleName,
                    Email = student.Email,
                    Password = student.Password
                };
                OperationDetails result = await StudentService.AddAsync(studentDTO);
                string message = result.Message + "<br>";

                if (result.Succedeed)
                {
                    TempData["message"] += message;

                    result = GroupService.AddStudentToGroupByEmail(student.IdGroup, student.Email);
                    message = result.Message + "<br>";

                    if (result.Succedeed)
                    {
                        TempData["message"] += message;

                        List<StudentSubjectDTO> studentSubjectsDTO = new List<StudentSubjectDTO>();

                        length = student.SelectedIdSubjects.Count;//save count selectedIdSubjects
                        if (length == selectedIdTeach.Count)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                result = await StudentSubjectService.AddByEmail(student.Email, new StudentSubjectDTO()
                                {
                                    SubjectId = student.SelectedIdSubjects[i],
                                    TeacherId = selectedIdTeach[i]
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
                    }
                }
                else
                {
                    TempData["errorMessage"] += message;
                }

                return Redirect($@"Details/{(await StudentService.GetByEmailAsync(student.Email)).Id}");
            }

            //apply mega shit
            student.Groups = GetAllGroups(student.FacultyId);
            student.Subjects = GetAllSubjects(student.FacultyId);
            student.SubjectsWithTeachers = GetAllSubjectsWithTeachers(student.FacultyId).ToList();

            return View(student);
        }


        private List<SubjectWithTeachers> GetAllSubjectsWithTeachers(string idFaculty)
        {
            return GetAllSubjectsWithTeachers(SubjectService.GetSubjectsInFaculty(idFaculty).Select(s => s.Id));
        }

        private List<SubjectWithTeachers> GetAllSubjectsWithTeachers(IEnumerable<string> subjectsId)
        {
            List<SubjectWithTeachers> result = new List<SubjectWithTeachers>();
            foreach (var item in subjectsId)
            {
                var subject = BaseViewModel.UniversalConvert<SubjectDTO, SubjectWithTeachers>(SubjectService.Get(item));
                var teachers = BaseViewModel.UniversalConvert<TeacherDTO, TeacherViewModel>(TeacherService.GetTeachersBySubjectId(item));

                if (teachers.Any())
                {
                    subject.Teachers = teachers.ToList();
                    result.Add(subject);
                }
            }

            return result;
        }

        //mega shit!!!!
        private List<SubjectViewModel> GetAllSubjects(string idFaculty) =>
            BaseViewModel.UniversalConvert<SubjectDTO, SubjectViewModel>(SubjectService.GetSubjectsInFaculty(idFaculty)).ToList();

        //mega shit too!!!!
        private List<GroupViewModel> GetAllGroups(string idFaculty) =>
            BaseViewModel.UniversalConvert<GroupDTO, GroupViewModel>(GroupService.GetGroupsInFaculty(idFaculty)).ToList();

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
    }
}