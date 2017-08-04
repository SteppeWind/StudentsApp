using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.EF;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Repositories
{
    public class DeanRepository : AbstractDeanRepository<Dean>
    {
        private BaseRepository<Dean> baseRepository;
        private BaseRepository<DeanFaculty> deanFacultyReposutory;

        private AbstractProfileRepository<Profile> profileRepository;
        protected override AbstractProfileRepository<Profile> ProfileRepository => profileRepository;

        protected StudentsAppContext dbContext { get; set; }

        public DeanRepository(StudentsAppContext context)
        {
            dbContext = context;
            baseRepository = new BaseRepository<Dean>(dbContext);
            profileRepository = new ProfileRepository<Profile>(dbContext);
            deanFacultyReposutory = new BaseRepository<DeanFaculty>(dbContext);
        }

        public override IEnumerable<Dean> GetAll => baseRepository.GetAll;
       
        public override void Add(Dean entity)
        {
            baseRepository.Add(entity);
        }

        public override IEnumerable<Dean> Find(Func<Dean, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public override void FullRemove(Dean entity)
        {
            baseRepository.FullRemove(entity);
            profileRepository.FullRemove(entity.Id);
        }

        public override Dean GetById(int id)
        {
            return baseRepository.GetById(id);
        }

        public override void Remove(Dean entity)
        {
            baseRepository.Remove(entity);
        }

        public override void Update(Dean entity)
        {
            baseRepository.Update(entity);
        }

        public override void AddFaculty(Dean dean, int idFaculty)
        {
            deanFacultyReposutory.Add(new DeanFaculty()
            {
                DeanId = dean.Id,
                FacultyId = idFaculty,
                StartManage = DateTime.Now
            });
        }

        public override void RemoveFaculty(Dean dean, int idFaculty)
        {
            var deanFaculty = dean.ListDeanFaculties
                .FirstOrDefault(d => d.FacultyId == idFaculty && d.DeanId == dean.Id && d.EndManage != null);
            deanFaculty.EndManage = DateTime.Now;
        }
    }
}