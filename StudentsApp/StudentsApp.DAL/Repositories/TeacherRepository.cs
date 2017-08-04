using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.EF;
using StudentsApp.DAL.Contracts;
using System.Data.Entity;

namespace StudentsApp.DAL.Repositories
{
    public class TeacherRepository : AbstractTeacherRepository<Teacher>
    {
        private BaseRepository<TeacherFaculty> teacherFacultyRepository;
        private SubjectRepository subjectRepository;
        private BaseRepository<Teacher> baseRepository;

        private AbstractProfileRepository<Profile> profileRepository;
        protected override AbstractProfileRepository<Profile> ProfileRepository => profileRepository;

        protected StudentsAppContext dbContext { get; set; }

        public TeacherRepository(StudentsAppContext context)
        {
            dbContext = context;
            teacherFacultyRepository = new BaseRepository<TeacherFaculty>(dbContext);
            subjectRepository = new SubjectRepository(dbContext);
            baseRepository = new BaseRepository<Teacher>(dbContext);
            profileRepository = new ProfileRepository<Profile>(dbContext);
        }

        public override IEnumerable<Teacher> GetAll => baseRepository.GetAll;

        public override void Add(Teacher entity)
        {
            baseRepository.Add(entity);
        }

        public override IEnumerable<Teacher> Find(Func<Teacher, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public override void FullRemove(Teacher entity)
        {
            baseRepository.FullRemove(entity);
            profileRepository.FullRemove(entity.Id);
        }

        public override Teacher GetById(int id)
        {
            return baseRepository.GetById(id);
        }

        public override void Remove(Teacher entity)
        {
            baseRepository.Remove(entity);
        }
        

        public override void Update(Teacher entity)
        {
            baseRepository.Update(entity);
        }

        public override void AddSubject(Teacher teacher, int idSubject)
        {
            teacher.ListSubjects.Add(subjectRepository[idSubject]);
        }

        public override void RemoveSubject(Teacher teacher, int idSubject)
        {
            teacher.ListSubjects.Remove(subjectRepository[idSubject]);
        }

        public override void AddTeacherPost(int idTeacher, int idFaculty, int idPost)
        {
            teacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = idFaculty,
                PostTeacherId = idPost,
                TeacherId = idTeacher
            });
        }

        public override void RemoveTeacherPost(int idTeacher, int idFaculty, int idPost)
        {
            var teacherFaculty = teacherFacultyRepository
                   .Find(t => t.TeacherId == idTeacher && t.PostTeacherId == idPost && t.FacultyId == idFaculty);

            teacherFacultyRepository.Remove(teacherFaculty);
        }

        public override void UpdateTeacherPost(int idTeacher, int idFaculty, int idOldPost, int idNewPost)
        {
            var teacherFaculty = teacherFacultyRepository
                  .Find(t => t.TeacherId == idTeacher && t.PostTeacherId == idOldPost && t.FacultyId == idFaculty)
                  .First();
            teacherFaculty.PostTeacherId = idNewPost;

            teacherFacultyRepository.Update(teacherFaculty);
        }

        public override IEnumerable<Teacher> GetTeachers(int idFaculty)
        {
            return baseRepository.GetAll.Where(t => t.ListTeacherFaculty.Select(tf => tf.FacultyId).Contains(idFaculty));
        }
    }
}