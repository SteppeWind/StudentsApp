using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.EF;
using StudentsApp.DAL.Contracts;

namespace StudentsApp.DAL.Repositories
{
    public class ProfileRepository<TEntity> : AbstractProfileRepository<TEntity> where TEntity : Profile
    {
        private BaseRepository<TEntity> baseRepository;
        protected StudentsAppContext dbContext { get; set; }

        public override IEnumerable<TEntity> GetAll => baseRepository.GetAll;

        public ProfileRepository(StudentsAppContext context)
        {
            dbContext = context;
            baseRepository = new BaseRepository<TEntity>(dbContext);
        }

        public override IEnumerable<TEntity> GetByMiddleName(string middleName)
        {
            return Find(p => p.MiddleName.ToUpper().Contains(middleName.ToUpper()));
        }

        public override IEnumerable<TEntity> GetByName(string name)
        {
            return Find(p => p.Name.ToUpper().Contains(name.ToUpper()));
        }

        public override IEnumerable<TEntity> GetBySurname(string surname)
        {
            return Find(p => p.Surname.ToUpper().Contains(surname.ToUpper()));
        }

        public override TEntity GetByEmail(string email)
        {
            return Find(p => p.Email.ToUpper().Equals(email.ToUpper())).FirstOrDefault();
        }

        public override void Add(TEntity entity)
        {
            baseRepository.Add(entity);
        }

        public override void Remove(TEntity entity)
        {
            baseRepository.Remove(entity);
        }

        public override void FullRemove(TEntity entity)
        {
            baseRepository.FullRemove(entity);
        }

        public override void Update(TEntity entity)
        {
            baseRepository.Update(entity);
        }

        public override IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public override TEntity GetById(int id)
        {
            return baseRepository.GetById(id);
        }
    }
}