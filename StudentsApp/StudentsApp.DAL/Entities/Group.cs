using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class Group : BaseEntity
    {
        public string GroupName { get; set; }


        public virtual ICollection<Student> ListStudents { get; set; }


        public int FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }


        public Group()
        {
            ListStudents = new List<Student>();
        }
    }
}