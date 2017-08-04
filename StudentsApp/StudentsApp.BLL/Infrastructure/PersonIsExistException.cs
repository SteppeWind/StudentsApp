using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Infrastructure
{
    public class PersonIsExistException : Exception
    {
        public PersonDTO Person { get; private set; }

        public override string Message { get; }

        public PersonIsExistException(PersonDTO person, string message = "") : base(message)
        {
            if (string.IsNullOrEmpty(message))
                Message = $"Пользователь с email {person.Email} уже существует";

            this.Person = person;
        }
    }
}