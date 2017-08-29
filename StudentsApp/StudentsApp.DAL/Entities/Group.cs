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


        public virtual ICollection<StudentGroup> ListStudents { get; set; }


        public string FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }


        public Group()
        {
            ListStudents = new List<StudentGroup>();
        }
    }
}