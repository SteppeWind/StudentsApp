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
        public StudentService(IUnitOfWork uow) : base(uow) { }

        private Subject GetSubjectIfExist(string id)
        {
            var subject = DataBase.SubjectRepository[id];

            if (subject == null)
            {
                throw new ValidationException("Предмет не найден");
            }

            return subject;
        }

        private Teacher GetTeacherIfExist(string id)
        {
            var teacher = DataBase.TeacherRepository[id];

            if (teacher == null)
            {
                throw new ValidationException("Преподаватель не найден");
            }

            return teacher;
        }

        private Student GetStudentIfExistById(string id)
        {
            var student = DataBase.StudentRepository[id];

            if (student == null)
            {
                throw new ValidationException("Студент не найден");
            }

            return student;
        }

        private Student GetStudentIfExistByEmail(string email)
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
            result.ListIdGroups = entity.ListGroups.Select(g => g.GroupId);
            //save id`s marks
            result.ListIdMarks = entity.ListMarks.Select(m => m.Id);
            //save id`s subjects
            result.ListIdSubjects = entity.ListVisitSubjects.Select(s => s.SubjectId);

            return result;
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

        public int Count => DataBase.StudentRepository.Count;

        /// <summary>
        /// Добавляет нового студента
        /// </summary>
        /// <param name="entity">Студент</param>
        /// <returns></returns>
        public async Task<OperationDetails> AddAsync(StudentDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                bool isExist = PersonIsExist(entity);//find profile with same emai

                if (isExist)//if exist
                {
                    answer = new OperationDetails(false, $"Студент с email '{entity.Email}' уже существует");
                }
                else
                {
                    var student = await Create<DAL.Entities.Profile>(entity);
                    DataBase.StudentRepository.Add(student);//add to DB
                    await DataBase.SaveAsync();

                    await DataBase.ProfileManager.AddToRoleAsync(student.Id, "student");
                    await DataBase.SaveAsync();

                    answer = new OperationDetails(true, $"Студент '{entity.FullName}' успешно зарегистрирован");
                }
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }
         
            return answer;
        }

        /// <summary>
        /// Change value IsDelete to false for finded student
        /// </summary>
        /// <param name="idStudent"></param>
        /// <returns></returns>
        public OperationDetails Remove(string idStudent)
        {
            OperationDetails answer = null;

            try
            {
                var student = GetStudentIfExistById(idStudent);

                DataBase.StudentRepository.Remove(student.Id);
                DataBase.Save();
                answer = new OperationDetails(true, $"Студент {student.Profile.ToString()} помечен как 'Удалён'");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Full delete student from DB 
        /// </summary>
        /// <param name="idStudent">Id student</param>
        /// <returns></returns>
        public OperationDetails FullRemove(string idStudent)
        {
            OperationDetails answer = null;

            try
            {
                var student = GetStudentIfExistById(idStudent);//find student

                DataBase.StudentGroupRepository.FullRemove(student.ListGroups);
                DataBase.StudentSubjectRepository.FullRemove(student.ListVisitSubjects);
                DataBase.MarkRepository.FullRemove(student.ListMarks);
                answer = new OperationDetails(true, $"Студент {student.Profile.ToString()} полностью удален из базы");
                Task.Run(async () => await DeleteProfile(student.Profile));
                DataBase.StudentRepository.FullRemove(idStudent);
                DataBase.Save();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }      

        public async Task<OperationDetails> UpdateAsync(StudentDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                if (PersonIsExist(entity))
                {
                    answer = new OperationDetails(false, $"Профайл с emal {entity.Email} уже существует");
                }
                else
                {
                    var student = GetStudentIfExistById(entity.Id);
                    var profile = student.Profile;
                    UpdatePerson(entity, profile);
                    var result = await DataBase.ProfileManager.UpdateAsync(profile);//update profile
                    await DataBase.SaveAsync();
                    answer = new OperationDetails(true, $"Профиль студента успешно обновлен");                    
                }
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }
            
            return answer;
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id">Id student</param>
        /// <returns></returns>
        public StudentDTO Get(string id)
        {
            return Convert(GetStudentIfExistById(id));
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

        ///// <summary>
        ///// Add subjects to student
        ///// </summary>
        ///// <param name="idStudent"></param>
        ///// <param name="idsSubjects"></param>
        ///// <param name="teacherId"></param>
        //public void AddSubject(int idStudent, IEnumerable<int> idsSubjects, int teacherId)
        //{
        //    //здесь ситуация следующая
        //    //передаем сюда список айдишников предметов и айди преподавателя
        //    //подразумевается, что список предметов ведётся данным преподавателем
        //    var student = GetStudentIfExist(idStudent);

        //    //find ids subjects which student doesn`t study
        //    var exceptSubjects = idsSubjects.Except(student.ListVisitSubjects.Select(s => s.SubjectId)).ToList();
        //    if (exceptSubjects.Any())//if any
        //    {
        //        //add these subject to student
        //        int length = exceptSubjects.Count;
        //        for (int i = 0; i < length; i++)
        //        {
        //            var subjectId = exceptSubjects[i];
        //            AddSubject(student, subjectId, teacherId);
        //        }
        //    }
        //}

        /// <summary>
        /// Get students from faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<StudentDTO> GetStudents(string idFaculty)
        {
            var faculty = DataBase.FacultyRepository[idFaculty];
            List<StudentDTO> students = new List<StudentDTO>();

            foreach (var student in DataBase.StudentRepository.GetAll)
            {
                foreach (var group in DataBase.GroupRepository.GetAll)
                {
                    if (group.FacultyId == idFaculty && group.ListStudents.Intersect(student.ListGroups).Any())
                    {
                        students.Add(Convert(student));
                    }
                }
            }

            return students;
        }

        /// <summary>
        /// Get students whos visit teacher on individual subjet (which have teacher)  
        /// </summary>
        /// <param name="idSubject">Id subject</param>
        /// <param name="idTeacher">Id teacher</param>
        /// <returns>Students</returns>
        public IEnumerable<StudentDTO> GetStudents(string idSubject, string idTeacher)
        {
            var subject = GetSubjectIfExist(idSubject);
            var teacher = GetTeacherIfExist(idTeacher);

            var studentsDTO = Convert(subject.ListVisitSubjects
                .Intersect(teacher.ListStudents)
                .Select(vs => vs.Student));//find intersect teacher and student subjects and return Student

            return studentsDTO;
        }

        /// <summary>
        /// Get students whose average mark more middle mark
        /// </summary>
        /// <param name="idFaculty"></param>
        /// <returns></returns>
        public IEnumerable<StudentDTO> GetStudentsMoreMiddleAverageMark(string idFaculty)
        {
            List<StudentDTO> students = GetStudents(idFaculty).ToList();

            var averageMarkAllStudents = students.Average(s => s.AverageMark);//find middle average mark

            return students.Where(s => s.AverageMark > averageMarkAllStudents);
        }
    }
}