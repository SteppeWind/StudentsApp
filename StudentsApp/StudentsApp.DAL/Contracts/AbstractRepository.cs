using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractRepository<TEntity> where TEntity : IBaseEntity
    {
        public abstract void Add(TEntity entity);

        public abstract void Remove(TEntity entity);

        public abstract void FullRemove(TEntity entity);

        public abstract void Update(TEntity entity);

        public abstract IEnumerable<TEntity> GetAll { get; }

        public abstract IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);

        public abstract TEntity FindFirst(Func<TEntity, Boolean> predicate);

        public abstract TEntity GetById(string id);

        public abstract int Count { get; }

        public TEntity this[string id] => GetById(id);

        public void Remove(string id) => Remove(this[id]);

        public void Remove(IEnumerable<TEntity> rangeEntity)
        {
            foreach (var item in rangeEntity)
            {
                Remove(item);
            }
        }

        public void FullRemove(string id) => FullRemove(this[id]);

        public void FullRemove(IEnumerable<TEntity> rangeEntity)
        {
            foreach (var item in rangeEntity)
            {
                FullRemove(item);
            }
        }      
    }
}