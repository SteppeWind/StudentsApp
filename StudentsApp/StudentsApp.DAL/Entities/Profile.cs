using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class Profile : BaseEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Surname} {Name} {MiddleName}";
        }
    }
}