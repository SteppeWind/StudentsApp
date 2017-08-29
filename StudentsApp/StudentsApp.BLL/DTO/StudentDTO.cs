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

        public IEnumerable<string> ListIdGroups { get; set; }

        public IEnumerable<string> ListIdSubjects { get; set; }

        public IEnumerable<string> ListIdMarks { get; set; }

        public double AverageMark { get; set; }

        public StudentDTO()
        {
            ListIdGroups = new List<string>();
            ListIdSubjects = new List<string>();
            ListIdMarks = new List<string>();
            Role = "student";
        }
    }
}