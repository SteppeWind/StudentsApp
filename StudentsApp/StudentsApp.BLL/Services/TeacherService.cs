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
using System.Collections;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Service for work with teachers
    /// </summary>
    public class TeacherService : PersonService<TeacherDTO, Teacher>, ITeacherService
    {
        public TeacherService(IUnitOfWork uow, IIndentityUnitOfWork iouw) : base(uow, iouw) { }


        private Teacher GetTeacherIfExist(int id)
        {
            var teacher = DataBase.TeacherRepository[id];

            if (teacher == null)
            {
                throw new ValidationException("Преподаватель не найден");
            }

            return teacher;
        }

        private Teacher GetTeacherIfExist(string email)
        {
            var teacher = DataBase.TeacherRepository.GetByEmail(email);

            if (teacher == null)
            {
                throw new ValidationException("Преподаватель не найден");
            }

            return teacher;
        }

        private Subject GetSubjectIfExist(int id)
        {
            var subject = DataBase.SubjectRepository[id];

            if (subject == null)
            {
                throw new ValidationException("Дисциплина не найдена");
            }

            return subject;
        }

        private Faculty GetFacultyIfExist(int id)
        {
            var faculty = DataBase.FacultyRepository[id];

            if (faculty == null)
            {
                throw new ValidationException("Факультет не найден");
            }

            return faculty;
        }



        protected override TeacherDTO Convert(Teacher entity)
        {
            var result = base.Convert(entity);

            result.ListIdSubjects = entity.ListSubjects?.Select(s => s.Id).ToList();

            result.ListIdStudents = entity.ListVisitSubjects?.Select(s => s.StudentId).ToList();

            result.ListIdFaculties = entity.ListTeacherFaculty?.Select(tf => tf.FacultyId).ToList();

            //find intersect with studentId and teacherId, remove duplicates and save count
            if (entity.ListVisitSubjects != null)
                result.CountStudents = entity.ListVisitSubjects.Select(vs => new { stId = vs.StudentId, tchId = vs.TeacherId }).Distinct().Count();

            return result;
        }

        /// <summary>
        /// Get all teachers in DB
        /// </summary>
        public IEnumerable<TeacherDTO> GetAll
        {
            get
            {
                return Convert(DataBase.TeacherRepository.GetAll);
            }
        }

        /// <summary>
        /// Add teacher to DB
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TeacherDTO entity)
        {
            bool isExist = PersonIsExist(entity);//find the same email
            if (isExist)
            {
                throw new PersonIsExistException(entity, $"Пользователь с email {entity.Email} уже существует");
            }

            var teacher = ReverseConvert(entity);
            var profile = UniversalReverseConvert<DAL.Entities.Profile, TeacherDTO>(entity);
            teacher.Profile = profile;

            DataBase.TeacherRepository.Add(teacher);
            Create(entity);

            DataBase.Save();
        }

        /// <summary>
        /// Add subject to teacher
        /// </summary>
        /// <param name="idSubject">Id subject</param>
        /// <param name="idTeacher">Id teacher</param>
        public void AddSubject(int idSubject, int idTeacher)
        {
            var teacher = GetTeacherIfExist(idTeacher);
            var subject = GetSubjectIfExist(idSubject);

            if (teacher.ListSubjects.Contains(subject))
            {
                throw new ValidationException($"Преподаватель {teacher.Profile} уже преподает дисцпилину {subject.SubjectName}");
            }

            AddSubject(teacher, subject.Id);
        }

        private void AddTeacherPost(Teacher teacher, int idFaculty, int idPost)
        {
            DataBase.TeacherRepository.AddTeacherPost(teacher, idFaculty, idPost);
            DataBase.Save();
        }

        private void AddSubject(Teacher teacher, int idSubject)
        {
            DataBase.TeacherRepository.AddSubject(teacher, idSubject);
            DataBase.Save();
        }


        public void AddTeacherPost(int idTeacher, int idFaculty, int idPost)
        {
            var teacher = GetTeacherIfExist(idTeacher);
            var faculty = GetFacultyIfExist(idFaculty);
            var post = DataBase.PostTeacherRepository[idPost];

            if (post == null)
            {
                //check
            }

            AddTeacherPost(teacher, idFaculty, idPost);
        }

        public void FullRemove(int id)
        {
            var teacher = GetTeacherIfExist(id);

            DataBase.MarkRepository.FullRemove(teacher.ListMarks);
            DataBase.TeacherFacultyRepository.FullRemove(teacher.ListTeacherFaculty);
            DataBase.VisitSubjectRepository.FullRemove(teacher.ListVisitSubjects);
            Delete(teacher);//delete teacher from IdentityDB
            DataBase.TeacherRepository.FullRemove(teacher);
            DataBase.Save();
        }

        /// <summary>
        /// Get teacher by id
        /// </summary>
        /// <param name="id">Id teacher</param>
        /// <returns></returns>
        public TeacherDTO Get(int id)
        {
            return Convert(GetTeacherIfExist(id));
        }

        /// <summary>
        /// Change value IsDelete to false for finded teacher
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            var teacher = GetTeacherIfExist(id);

            DataBase.TeacherRepository.Remove(teacher);
            DataBase.Save();
        }

        /// <summary>
        /// Delete subject from teacher
        /// </summary>
        /// <param name="idSubject">Id subject</param>
        /// <param name="idTeacher">Id teacher</param>
        public void RemoveSubject(int idSubject, int idTeacher)
        {
            var teacher = GetTeacherIfExist(idTeacher);
            var subject = GetSubjectIfExist(idSubject);

            DataBase.TeacherRepository.RemoveSubject(teacher, subject.Id);
            DataBase.Save();
        }

        /// <summary>
        /// Remove post from teacher
        /// </summary>
        /// <param name="idTeacher">Id teacher</param>
        /// <param name="idFaculty">Id faculty</param>
        /// <param name="idPost">Id post</param>
        public void RemoveTeacherPost(int idTeacher, int idFaculty, int idPost)
        {
            var teacher = GetTeacherIfExist(idTeacher);
            var faculty = GetFacultyIfExist(idFaculty);
            var post = DataBase.PostTeacherRepository[idPost];

            if (post == null)
            {
                //check
            }

            DataBase.TeacherRepository.RemoveTeacherPost(idTeacher, idFaculty, idPost);
            DataBase.Save();
        }

        public void Update(TeacherDTO entity)
        {
            var teacher = GetTeacherIfExist(entity.Id);

            var profile = teacher.Profile;
            UpdatePerson(entity, profile);//update profile
            DataBase.ProfileRepository.Update(profile);
            DataBase.Save();
        }

        public void UpdateTeacherPost(int idTeacher, int idFaculty, int idOldPost, int idNewPost)//*?
        {
            var teacher = DataBase.TeacherRepository[idTeacher];
            var faculty = DataBase.FacultyRepository[idFaculty];
            var oldPost = DataBase.PostTeacherRepository[idOldPost];
            var newPost = DataBase.PostTeacherRepository[idNewPost];

            if (teacher == null)
            {
                //check
            }

            if (faculty == null)
            {
                //check
            }

            if (oldPost == null)
            {
                //check
            }

            if (newPost == null)
            {
                //check
            }

            DataBase.TeacherRepository.UpdateTeacherPost(idTeacher, idFaculty, idOldPost, idNewPost);
            DataBase.Save();
        }

        public Task<TeacherDTO> GetByEmailAsync(string email)
        {
            var teacher = DataBase.TeacherRepository.GetByEmail(email);

            if (teacher == null)
            {
                throw new PersonNotFoundException(null, email, $"Профиль по email среди учителей {email} не найден");
            }

            return ConvertAsync(teacher);
        }

        public Task<IEnumerable<TeacherDTO>> GetByMiddleNameAsync(string middleName)
        {
            return ConvertAsync(DataBase.TeacherRepository.GetByMiddleName(middleName));
        }

        public Task<IEnumerable<TeacherDTO>> GetByNameAsync(string name)
        {
            return ConvertAsync(DataBase.TeacherRepository.GetByName(name));
        }

        public Task<IEnumerable<TeacherDTO>> GetBySurnameAsync(string surname)
        {
            return ConvertAsync(DataBase.TeacherRepository.GetBySurname(surname));
        }

        public IEnumerable<TeacherFacultyDTO> GetPosts(int idTeacher)
        {
            var teacher = DataBase.TeacherRepository[idTeacher];
            var teacherFaculties = DataBase.TeacherFacultyRepository.Find(t => t.TeacherId == idTeacher).ToList();
            var teacherFacultiesDTO = UniversalConvert<TeacherFaculty, TeacherFacultyDTO>(teacherFaculties).ToList();

            for (int i = 0; i < teacherFaculties.Count; i++)
            {
                var currTFDTO = teacherFacultiesDTO[i];
                var currTF = teacherFaculties[i];

                currTFDTO.FacultyName = currTF.Faculty.FacultyName;
                currTFDTO.NamePostTeacher = currTF.Post.NamePostTeacher;
            }

            return teacherFacultiesDTO;
        }

        public void AddSubject(int idSubject, string teacherEmail)
        {
            var teacher = GetTeacherIfExist(teacherEmail);

            AddSubject(teacher, idSubject);
        }


        public void AddTeacherPost(string teacherEmail, int idFaculty, int idPost)
        {
            var teacher = GetTeacherIfExist(teacherEmail);
            AddTeacherPost(teacher, idFaculty, idPost);
        }

        public void AddSubject(IEnumerable<int> idsSubjects, string teacherEmail)
        {
            var teacher = GetTeacherIfExist(teacherEmail);

            if (idsSubjects == null)
            {
                throw new NullReferenceException();
            }

            if (idsSubjects.Count() == 0)
            {
                throw new ValidationException("Вы не выбрали ни одного предмета");
            }

            foreach (var item in idsSubjects)
            {
                var subject = GetSubjectIfExist(item);
                AddSubject(teacher, subject.Id);
            }
        }

        /// <summary>
        /// Get teachers from faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<TeacherDTO> GetTeachers(int idFaculty)
        {
            List<TeacherDTO> teachers = new List<TeacherDTO>();

            foreach (var post in DataBase.TeacherFacultyRepository.GetAll)
            {
                foreach (var teacher in GetAll)
                {
                    if (post.TeacherId == teacher.Id && post.FacultyId == idFaculty)
                    {
                        teachers.Add(teacher);
                    }
                }
            }

            return teachers;
        }

        /// <summary>
        /// Get teachers with min count of students
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<TeacherDTO> GetTeachersWithMinCountStudents(int idFaculty)
        {
            List<TeacherDTO> teachers = GetTeachers(idFaculty).ToList();

            var minCountTeachers = teachers.Min(t => t.CountStudents);

            return teachers.Where(t => t.CountStudents == minCountTeachers);
        }

        /// <summary>
        /// Get teachers with max students visit
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<TeacherDTO> GetTeachersWithAllStudents(int idFaculty)
        {
            List<TeacherDTO> teachersDTO = new List<TeacherDTO>();
            List<Student> students = new List<Student>();

            var subjects = DataBase.SubjectRepository.GetAll;
            var teachers = DataBase.TeacherRepository.GetTeachers(idFaculty);

            //find students which visit one faculty
            foreach (var student in DataBase.StudentRepository.GetAll)
            {
                foreach (var group in DataBase.GroupRepository.GetAll)
                {
                    if (group.FacultyId == idFaculty && DataBase.GroupRepository.IsContaintsStudentFromGroup(group, student.Id))
                    {
                        students.Add(student);
                    }
                }
            }

            //если максимально просто, то ищем студентов у препода и если у них есть пересекающиеся дисциплины, то инкремент счетчика
            //если счетчик равен числу студентов на факультете, то этого препода посещают все студенты
            foreach (var teacher in teachers)
            {
                var teacherSubjects = teacher.ListSubjects;

                int count = 0;

                foreach (var student in students)
                {
                    var studentSubjects = student.ListVisitSubjects
                        .Where(vs => vs.TeacherId == teacher.Id).Select(vs => vs.Subject);

                    if (studentSubjects.Intersect(teacherSubjects).Any())
                    {
                        count++;
                    }
                }

                if (students.Count == count)
                {
                    teachersDTO.Add(Convert(teacher));
                }
            }

            return teachersDTO;
        }
    }
}