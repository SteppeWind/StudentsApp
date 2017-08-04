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
        public List<int> ListIdDeanFaculties { get; set; }

        public int FacultyId { get; set; }

        public DeanDTO()
        {
            ListIdDeanFaculties = new List<int>();
        }
    }
}