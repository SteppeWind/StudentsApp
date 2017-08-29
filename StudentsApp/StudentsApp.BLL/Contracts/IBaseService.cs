using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
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
        Task<OperationDetails> AddAsync(TEntity entity);
        OperationDetails Remove(string id);
        OperationDetails FullRemove(string id);
        Task<OperationDetails> UpdateAsync(TEntity entity);
        TEntity Get(string id);
        IEnumerable<TEntity> GetAll { get; }
        int Count { get; }
        //EntityDTO UniversalConvert<Entity, EntityDTO>(Entity entity) where Entity : BaseEntity where EntityDTO : BaseDTO;
        //Entity UniversalReverseConvert<Entity, EntityDTO>(EntityDTO entity) where Entity : BaseEntity where EntityDTO : BaseDTO;
    }
}