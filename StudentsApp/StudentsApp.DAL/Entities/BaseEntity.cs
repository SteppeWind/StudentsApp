using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public string Id { get; set; }

        public virtual bool IsDelete { get; set; }

        public static string GenerateId => Guid.NewGuid().ToString();

        public BaseEntity()
        {
            Id = GenerateId;
        }
    }
}