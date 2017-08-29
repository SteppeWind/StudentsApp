using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class StudentGroup : BaseEntity
    {
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }


        public string GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}