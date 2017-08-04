using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class StudentDTO : PersonDTO
    {
        //здесь пиздец

        public IEnumerable<int> ListIdGroups { get; set; }

        public IEnumerable<int> ListIdSubjects { get; set; }

        public IEnumerable<int> ListIdMarks { get; set; }

        public double AverageMark { get; set; }

        public StudentDTO()
        {
            ListIdGroups = new List<int>();
            ListIdSubjects = new List<int>();
            ListIdMarks = new List<int>();
            Role = "student";
        }
    }
}