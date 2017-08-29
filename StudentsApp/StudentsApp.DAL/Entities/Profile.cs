using Microsoft.AspNet.Identity.EntityFramework;
using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class Profile : IdentityUser, IDeleteEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        public bool IsDelete { get; set; }

        public override string ToString()
        {
            return $"{Surname} {Name} {MiddleName}";
        }
    }
}