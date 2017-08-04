using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractRepository<TEntity> where TEntity : BaseEntity
    {
        public abstract void Add(TEntity entity);

        public abstract void Remove(TEntity entity);

        public abstract void FullRemove(TEntity entity);

        public abstract void Update(TEntity entity);

        public abstract IEnumerable<TEntity> GetAll { get; }

        public abstract IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);       

        public abstract TEntity GetById(int id);

        public TEntity this[int id] => GetById(id);

        public void Remove(int id) => Remove(this[id]);

        public void Remove(IEnumerable<TEntity> rangeEntity)
        {
            foreach (var item in rangeEntity)
            {
                Remove(item);
            }
        }

        public void FullRemove(int id) => FullRemove(this[id]);

        public void FullRemove(IEnumerable<TEntity> rangeEntity)
        {
            foreach (var item in rangeEntity)
            {
                FullRemove(item);
            }
        }
    }
}
