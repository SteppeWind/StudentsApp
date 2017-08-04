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
    public class GroupRepository : AbstractGroupRepository<Group>
    {
        private StudentRepository studentRepository;
        private BaseRepository<Group> baseRepository;
        
        protected StudentsAppContext dbContext { get; set; }

        public GroupRepository(StudentsAppContext context)
        {
            dbContext = context;
            studentRepository = new StudentRepository(dbContext);
            baseRepository = new BaseRepository<Group>(dbContext);
        }

        public override IEnumerable<Group> GetAll => baseRepository.GetAll;

        public override void Add(Group entity)
        {
            baseRepository.Add(entity);
        }

        public override void AddStudentToGroup(Group group, int idStudent)
        {
            group.ListStudents.Add(studentRepository[idStudent]);
        }

        public override IEnumerable<Group> Find(Func<Group, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public override void FullRemove(Group entity)
        {
            baseRepository.FullRemove(entity);
        }

        public override Group GetById(int id)
        {
            return baseRepository.GetById(id);
        }

        public override void Remove(Group entity)
        {
            baseRepository.Remove(entity);
        }

        public override void RemoveStudentFromGroup(Group group, int idStudent)
        {
            group.ListStudents.Remove(studentRepository[idStudent]);
        }

        public override void Update(Group entity)
        {
            baseRepository.Update(entity);
        }

        public override void AddStudentToGroup(Group group, string email)
        {
            var st = studentRepository.GetByEmail(email);
            group.ListStudents.Add(st);
        }

        public override bool IsContaintsStudentFromGroup(Group group, int idStudent)
        {
            return group.ListStudents.Select(s => s.Id).Contains(idStudent);
        }
    }
}