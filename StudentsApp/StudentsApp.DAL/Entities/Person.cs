using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public abstract class Person : BaseEntity, IProfile
    {
        public virtual Profile Profile { get; set; }
    }
}