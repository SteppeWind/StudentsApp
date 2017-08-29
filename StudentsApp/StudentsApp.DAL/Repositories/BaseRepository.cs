using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.EF;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StudentsApp.DAL.Repositories
{
    public class BaseRepository<TEntity> : AbstractRepository<TEntity> where TEntity : BaseEntity
    {
        private DbSet<TEntity> entities { get; set; }
        protected StudentsAppContext dbContext { get; set; }

        public BaseRepository(StudentsAppContext context)
        {
            dbContext = context;
            entities = dbContext.Set<TEntity>();
        }

        public override void Add(TEntity entity)
        {
            entities.Add(entity);
        }

        public override void Remove(TEntity entity)
        {
            entity.IsDelete = true;
        }

        public override void FullRemove(TEntity entity)
        {
            entities.Remove(entity);
        }

        public override void Update(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public override IEnumerable<TEntity> GetAll => entities.ToList();

        public override int Count => entities.Count();

        public override IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return entities.Where(predicate).ToList();
        }

        public override TEntity GetById(string id)
        {
            return entities?.FirstOrDefault(e => e.Id == id);
        }

        public override TEntity FindFirst(Func<TEntity, bool> predicate)
        {
            return Find(predicate).FirstOrDefault();
        }
    }
}