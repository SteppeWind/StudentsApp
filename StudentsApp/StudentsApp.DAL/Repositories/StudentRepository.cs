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
    public class StudentRepository : AbstractStudentRepository<Student>
    {
        private BaseRepository<VisitSubject> visitSubjectRepository;
        private BaseRepository<Student> baseRepository;

        private AbstractProfileRepository<Profile> profileRepository;
        protected override AbstractProfileRepository<Profile> ProfileRepository => profileRepository;

        protected StudentsAppContext dbContext { get; set; }

        public StudentRepository(StudentsAppContext context)
        {
            dbContext = context;
            visitSubjectRepository = new BaseRepository<VisitSubject>(dbContext);
            baseRepository = new BaseRepository<Student>(dbContext);
            profileRepository = new ProfileRepository<Profile>(dbContext);
        }

        public override IEnumerable<Student> GetAll => baseRepository.GetAll;
        
        public override void Add(Student entity)
        {
            baseRepository.Add(entity);
        }

        public override IEnumerable<Student> Find(Func<Student, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public override void FullRemove(Student entity)
        {
            visitSubjectRepository.FullRemove(entity.ListVisitSubjects);  
            baseRepository.FullRemove(entity);
            profileRepository.FullRemove(entity.Id);
        }

        public override Student GetById(int id)
        {
            return baseRepository.GetById(id);
        }

        public override void Remove(Student entity)
        {
            baseRepository.Remove(entity);
        }


        public override void Update(Student entity)
        {
            baseRepository.Update(entity);
        }
        
        public override void AddSubject(Student student, int idSubject, int idTeacher)
        {
            student.ListVisitSubjects.Add(new VisitSubject()
            {
                TeacherId = idTeacher,
                SubjectId = idSubject
            });
        }

        public override void RemoveSubject(Student student, int idSubject)
        {
            var subj = visitSubjectRepository.Find(vs => vs.SubjectId == idSubject && vs.StudentId == student.Id).FirstOrDefault();
            visitSubjectRepository.FullRemove(subj);
            //student.ListVisitSubjects.Remove(subj); 
        }
    }
}