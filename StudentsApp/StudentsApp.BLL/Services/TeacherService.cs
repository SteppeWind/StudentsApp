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
using StudentsApp.BLL.Message;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Service for work with teachers
    /// </summary>
    public class TeacherService : PersonService<TeacherDTO, Teacher>, ITeacherService
    {
        public TeacherService(IUnitOfWork uow) : base(uow) { }

        private Teacher GetTeacherIfExistById(string id)
        {
            var teacher = DataBase.TeacherRepository[id];

            if (teacher == null)
            {
                throw new PersonNotFoundException(new TeacherMessage().NotFound());
            }

            return teacher;
        }

        private Teacher GetTeacherIfExistByEmail(string email)
        {
            var teacher = DataBase.TeacherRepository.GetByEmail(email);

            if (teacher == null)
            {
                throw new ValidationException(new TeacherMessage().NotFoundByEmail(email));
            }

            return teacher;
        }

        private Subject GetSubjectIfExist(string id)
        {
            var subject = DataBase.SubjectRepository[id];

            if (subject == null)
            {
                throw new ValidationException(new SubjectMessage().NotFound());
            }

            return subject;
        }

        private Faculty GetFacultyIfExist(string id)
        {
            var faculty = DataBase.FacultyRepository[id];

            if (faculty == null)
            {
                throw new ValidationException(new FacultyMessage().NotFound());
            }

            return faculty;
        }



        protected override TeacherDTO Convert(Teacher entity)
        {
            var result = base.Convert(entity);

            result.ListIdSubjects = entity.ListSubjects?.Select(s => s.SubjectId).ToList();

            result.ListIdStudents = entity.ListStudents?.Select(s => s.StudentId).ToList();

            result.ListIdFaculties = entity.ListTeacherFaculty?.Select(tf => tf.FacultyId).ToList();

            //find intersect with studentId and teacherId, remove duplicates and save count
            if (entity.ListStudents != null)
                result.CountStudents = entity.ListStudents.Select(vs => new { stId = vs.StudentId, tchId = vs.TeacherId }).Distinct().Count();

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

        public int Count => DataBase.TeacherRepository.Count;

        /// <summary>
        /// Add teacher to DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationDetails> AddAsync(TeacherDTO entity)
        {
            OperationDetails answer = null;

            bool isExist = PersonIsExist(entity);//find the same email
            if (isExist)
            {
                answer = new OperationDetails(false, new TeacherMessage().IsExist(entity.Email));
            }
            else
            {
                var teacher = await Create<DAL.Entities.Profile>(entity);               
                DataBase.TeacherRepository.Add(teacher);
                await DataBase.SaveAsync();

                await DataBase.ProfileManager.AddToRoleAsync(teacher.Id, "teacher");
                await DataBase.SaveAsync();

                answer = new OperationDetails(true, new TeacherMessage().Create(teacher.Profile.ToString()));
            }

            return answer;
        }

        public OperationDetails FullRemove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var teacher = GetTeacherIfExistById(id);

                DataBase.MarkRepository.FullRemove(teacher.ListMarks);
                DataBase.TeacherFacultyRepository.FullRemove(teacher.ListTeacherFaculty);
                DataBase.StudentSubjectRepository.FullRemove(teacher.ListStudents);
                DataBase.TeacherSubjectRepository.FullRemove(teacher.ListSubjects);
                answer = new OperationDetails(true, new TeacherMessage().FullRemove(teacher.Profile.ToString()));
                Task.Run(async () => await DeleteProfile(teacher.Profile));
                DataBase.TeacherRepository.FullRemove(teacher);
                DataBase.Save();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Get teacher by id
        /// </summary>
        /// <param name="id">Id teacher</param>
        /// <returns></returns>
        public TeacherDTO Get(string id)
        {
            return Convert(GetTeacherIfExistById(id));
        }

        /// <summary>
        /// Change value IsDelete to false for finded teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var teacher = GetTeacherIfExistById(id);

                DataBase.TeacherRepository.Remove(teacher);
                DataBase.Save();
                answer = new OperationDetails(true, new TeacherMessage().Remove(teacher.Profile.ToString()));
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public async Task<OperationDetails> UpdateAsync(TeacherDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var teacher = GetTeacherIfExistByEmail(entity.Id);

                var profile = teacher.Profile;
                UpdatePerson(entity, profile);//update profile
                await DataBase.ProfileManager.UpdateAsync(profile);
                await DataBase.SaveAsync();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public Task<TeacherDTO> GetByEmailAsync(string email)
        {
            var teacher = DataBase.TeacherRepository.GetByEmail(email);

            if (teacher == null)
            {
                throw new PersonNotFoundException(null, email, new TeacherMessage().NotFoundByEmail(email));
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

        public IEnumerable<TeacherFacultyDTO> GetPosts(string idTeacher)
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

        /// <summary>
        /// Get teachers from faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<TeacherDTO> GetTeachers(string idFaculty)
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
        public IEnumerable<TeacherDTO> GetTeachersWithMinCountStudents(string idFaculty)
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
        public IEnumerable<TeacherDTO> GetTeachersWithAllStudents(string idFaculty)
        {
            List<TeacherDTO> teachersDTO = new List<TeacherDTO>();
            List<Student> students = new List<Student>();

            var subjects = DataBase.SubjectRepository.GetAll;
            var teachers = DataBase.TeacherRepository.GetAll.Where(t => t.ListTeacherFaculty.Select(tf => tf.FacultyId).Contains(idFaculty));

            //find students which visit one faculty
            foreach (var student in DataBase.StudentRepository.GetAll)
            {
                foreach (var group in DataBase.GroupRepository.GetAll)
                {
                    if (group.FacultyId == idFaculty && group.ListStudents.Intersect(student.ListGroups).Any())
                    {
                        students.Add(student);
                    }
                }
            }

            //если максимально просто, то ищем студентов у препода и если у них есть пересекающиеся дисциплины, то инкремент счетчика
            //если счетчик равен числу студентов на факультете, то этого препода посещают все студенты
            foreach (var teacher in teachers)
            {
                var teacherSubjects = teacher.ListSubjects.Select(ts => ts.Subject);

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

        public IEnumerable<TeacherDTO> GetTeachersBySubjectId(string subjectId)
        {
            return Convert(DataBase.TeacherRepository.Find(t => t.ListSubjects.Select(s => s.SubjectId).Contains(subjectId)));
        }
    }
}