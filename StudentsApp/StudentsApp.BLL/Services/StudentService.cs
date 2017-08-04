using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using AutoMapper;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Service for work with students
    /// </summary>
    public class StudentService : PersonService<StudentDTO, Student>, IStudentService
    {
        public StudentService(IUnitOfWork uow, IIndentityUnitOfWork iuow) : base(uow, iuow) { }

        private Subject GetSubjectIfExist(int id)
        {
            var subject = DataBase.SubjectRepository[id];

            if (subject == null)
            {
                throw new ValidationException("Предмет не найден");
            }

            return subject;
        }

        private Teacher GetTeacherIfExist(int id)
        {
            var teacher = DataBase.TeacherRepository[id];

            if (teacher == null)
            {
                throw new ValidationException("Преподаватель не найден");
            }

            return teacher;
        }

        private Student GetStudentIfExist(int id)
        {
            var student = DataBase.StudentRepository[id];

            if (student == null)
            {
                throw new ValidationException("Студент не найден");
            }

            return student;
        }

        private Student GetStudentIfExist(string email)
        {
            var student = DataBase.StudentRepository.GetByEmail(email);

            if (student == null)
            {
                throw new ValidationException("Студент не найден");
            }

            return student;
        }

        protected override StudentDTO Convert(Student entity)
        {
            var result = base.Convert(entity);

            //save id`s groups
            result.ListIdGroups = entity.ListGroups.Select(g => g.Id);
            //save id`s marks
            result.ListIdMarks = entity.ListMarks.Select(m => m.Id);
            //save id`s subjects
            result.ListIdSubjects = entity.ListVisitSubjects.Select(s => s.SubjectId);

            return result;
        }

        /// <summary>
        /// Get students whos visit teacher on individual subjet (which have teacher)  
        /// </summary>
        /// <param name="idSubject">Id subject</param>
        /// <param name="idTeacher">Id teacher</param>
        /// <returns>Students</returns>
        public IEnumerable<StudentDTO> GetStudents(int idSubject, int idTeacher)
        {
            var subject = GetSubjectIfExist(idSubject);
            var teacher = GetTeacherIfExist(idTeacher);

            var studentsDTO = Convert(subject.ListVisitSubjects
                .Intersect(teacher.ListVisitSubjects)
                .Select(vs => vs.Student));//find intersect teacher and student subjects and return Student

            return studentsDTO;
        }

        /// <summary>
        /// Get all students in DB
        /// </summary>
        public IEnumerable<StudentDTO> GetAll
        {
            get
            {
                return Convert(DataBase.StudentRepository.GetAll);
            }
        }

        /// <summary>
        /// Добавляет нового студента
        /// </summary>
        /// <param name="entity">Студент</param>
        public void Add(StudentDTO entity)
        {
            bool isExist = PersonIsExist(entity);//find profile with same emai

            if (isExist)//if exist
            {
                throw new PersonIsExistException(entity);
            }

            var student = ReverseConvert(entity);//convert student to Entity DB
            var profile = UniversalReverseConvert<DAL.Entities.Profile, StudentDTO>(entity);//convert profile of student

            student.Profile = profile;//save profile
            DataBase.StudentRepository.Add(student);//add to DB
            Create(entity);//create in IdentityDB and save
            DataBase.Save();
        }

        /// <summary>
        /// Change value IsDelete to false for finded student
        /// </summary>
        /// <param name="idStudent"></param>
        public void Remove(int idStudent)
        {
            DataBase.StudentRepository.Remove(idStudent);
            DataBase.Save();
        }

        /// <summary>
        /// Full delete student from DB 
        /// </summary>
        /// <param name="idStudent">Id student</param>
        public void FullRemove(int idStudent)
        {
            var student = GetStudentIfExist(idStudent);//find student

            Delete(student);//delete from IdentityDB
            DataBase.StudentRepository.FullRemove(idStudent);
            DataBase.Save();
        }

        private void AddSubject(Student student, int idSubject, int idTeacher)
        {
            DataBase.StudentRepository.AddSubject(student, idSubject, idTeacher);
            DataBase.Save();
        }


        /// <summary>
        /// Добавляет предмет студенту, который он изучает
        /// </summary>
        /// <param name="idStudent">ID студента</param>
        /// <param name="idSubject">ID предмета</param>
        public void AddSubject(int idStudent, int idSubject, int idTeacher)
        {
            var student = GetStudentIfExist(idStudent);
            var subject = GetSubjectIfExist(idSubject);
            //finded visited subjects 
            var visitedSubject = DataBase.VisitSubjectRepository.Find(vs => subject.Id == vs.SubjectId && student.Id == vs.StudentId);

            if (visitedSubject.Any())//if any
            {
                throw new ValidationException($"Студент {student.Profile.ToString()} уже изучает дисциплину {subject.SubjectName}");
            }

            AddSubject(student, idSubject, idTeacher);
        }

        /// <summary>
        /// Удаляет предмет, который изучает студент
        /// </summary>
        /// <param name="idStudent">ID студента</param>
        /// <param name="idSubject">ID предмета</param>
        public void RemoveSubject(int idStudent, int idSubject)//*
        {
            var student = GetStudentIfExist(idStudent);
            var subject = GetSubjectIfExist(idSubject);

            DataBase.StudentRepository.RemoveSubject(student, subject.Id);
            DataBase.Save();
        }

        public void Update(StudentDTO entity)
        {
            var student = GetStudentIfExist(entity.Id);

            var profile = student.Profile;
            UpdatePerson(entity, profile);
            DataBase.ProfileRepository.Update(profile);//update profile
            DataBase.Save();
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id">Id student</param>
        /// <returns></returns>
        public StudentDTO Get(int id)
        {
            return Convert(GetStudentIfExist(id));
        }

        public Task<IEnumerable<StudentDTO>> GetByNameAsync(string name)
        {
            return ConvertAsync(DataBase.StudentRepository.GetByName(name));
        }

        public Task<IEnumerable<StudentDTO>> GetBySurnameAsync(string surname)
        {
            return ConvertAsync(DataBase.StudentRepository.GetBySurname(surname));
        }

        public Task<IEnumerable<StudentDTO>> GetByMiddleNameAsync(string middleName)
        {
            return ConvertAsync(DataBase.StudentRepository.GetByMiddleName(middleName));
        }

        public Task<StudentDTO> GetByEmailAsync(string email)
        {
            return ConvertAsync(DataBase.StudentRepository.GetByEmail(email));
        }

        /// <summary>
        /// Add subject to student by email
        /// </summary>
        /// <param name="emailStudent">Student email</param>
        /// <param name="idSubject">Id subject</param>
        /// <param name="idTeacher">Id teacher</param>
        public void AddSubject(string emailStudent, int idSubject, int idTeacher)
        {
            var student = GetStudentIfExist(emailStudent);
            AddSubject(student, idSubject, idTeacher);
        }

        /// <summary>
        /// Add subjects to student
        /// </summary>
        /// <param name="idStudent">Id student</param>
        /// <param name="idsSubjects">Id`s subjects</param>
        /// <param name="idsTeachers">Id`s students</param>
        public void AddSubject(int idStudent, IEnumerable<int> idsSubjects, IEnumerable<int> idsTeachers)
        {
            //мой уровень английского днищеват, поэтому опишу по русски
            //здесь мы передаем айди студента, айдишники предметов и преподавателей
            //вызываем этот метод в случае, когда мы выбрали несколько предметов и у каждого указали преподавателя, который будет вести
            //логически можно догадаться, сколько предметов, столько и преподавателй выбрали
            //поэтому каждому id предмета соответствует id преподавателя
            int length = idsSubjects.Count();//save length of subjects
            if (length == idsTeachers.Count())
            {
                for (int i = 0; i < length; i++)
                {
                    AddSubject(idStudent, idsSubjects.ElementAt(i), idsTeachers.ElementAt(i));
                }
            }
        }

        /// <summary>
        /// Add subjects to student 
        /// </summary>
        /// <param name="emailStudent">Student email</param>
        /// <param name="idsSubjects"></param>
        /// <param name="idsTeachers"></param>
        public void AddSubject(string emailStudent, IEnumerable<int> idsSubjects, IEnumerable<int> idsTeachers)
        {
            var student = GetStudentIfExist(emailStudent);

            if (idsSubjects.Count() == 0)
            {
                throw new ValidationException("Вы не выбрали ни одного предмета");
            }

            AddSubject(student.Id, idsSubjects, idsTeachers);
        }

        /// <summary>
        /// Add subjects to student
        /// </summary>
        /// <param name="idStudent"></param>
        /// <param name="idsSubjects"></param>
        /// <param name="teacherId"></param>
        public void AddSubject(int idStudent, IEnumerable<int> idsSubjects, int teacherId)
        {
            //здесь ситуация следующая
            //передаем сюда список айдишников предметов и айди преподавателя
            //подразумевается, что список предметов ведётся данным преподавателем
            var student = GetStudentIfExist(idStudent);

            //find ids subjects which student doesn`t study
            var exceptSubjects = idsSubjects.Except(student.ListVisitSubjects.Select(s => s.SubjectId)).ToList();
            if (exceptSubjects.Any())//if any
            {
                //add these subject to student
                int length = exceptSubjects.Count;
                for (int i = 0; i < length; i++)
                {
                    var subjectId = exceptSubjects[i];
                    AddSubject(student, subjectId, teacherId);
                }
            }
        }

        /// <summary>
        /// Get students from faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<StudentDTO> GetStudents(int idFaculty)
        {
            var faculty = DataBase.FacultyRepository[idFaculty];
            List<StudentDTO> students = new List<StudentDTO>();

            foreach (var student in DataBase.StudentRepository.GetAll)
            {
                foreach (var group in DataBase.GroupRepository.GetAll)
                {
                    if (group.FacultyId == idFaculty && DataBase.GroupRepository.IsContaintsStudentFromGroup(group, student.Id))
                    {
                        students.Add(Convert(student));
                    }
                }
            }

            return students;
        }

        /// <summary>
        /// Get students whose average mark more middle mark
        /// </summary>
        /// <param name="idFaculty"></param>
        /// <returns></returns>
        public IEnumerable<StudentDTO> GetStudentsMoreMiddleAverageMark(int idFaculty)
        {
            List<StudentDTO> students = GetStudents(idFaculty).ToList();

            var averageMarkAllStudents = students.Average(s => s.AverageMark);//find middle average mark

            return students.Where(s => s.AverageMark > averageMarkAllStudents);
        }
    }
}