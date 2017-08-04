using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class PersonDTO : BaseDTO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        public string Email { get; set; }        

        public string Password { get; set; }

        public string Role { get; set; }

        public string FullName => $"{Surname} {Name} {MiddleName}";
    }
}