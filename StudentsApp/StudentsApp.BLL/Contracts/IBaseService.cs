using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IBaseService<TEntity> where TEntity : BaseDTO
    {
        void Add(TEntity entity);
        void Remove(int id);
        void FullRemove(int id);
        void Update(TEntity entity);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll { get; }
        //EntityDTO UniversalConvert<Entity, EntityDTO>(Entity entity) where Entity : BaseEntity where EntityDTO : BaseDTO;
        //Entity UniversalReverseConvert<Entity, EntityDTO>(EntityDTO entity) where Entity : BaseEntity where EntityDTO : BaseDTO;
    }
}