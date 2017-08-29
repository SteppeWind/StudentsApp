using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Infrastructure
{
    public class PersonNotFoundException : Exception
    {
        public int? IdPerson { get; private set; }

        public string Email { get; private set; }

        public PersonNotFoundException(int? id, string email, string message) : base(message)
        {
            IdPerson = id;
            Email = email;
        }

        public PersonNotFoundException(string message) : base(message) { }
    }
}
