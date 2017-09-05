using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class DeanDTO : PersonDTO
    {
        public List<string> ListIdDeanFaculties { get; set; }

        public string FacultyId { get; set; }

        public DeanDTO()
        {
            Role = "dean";
            ListIdDeanFaculties = new List<string>();
        }
    }
}