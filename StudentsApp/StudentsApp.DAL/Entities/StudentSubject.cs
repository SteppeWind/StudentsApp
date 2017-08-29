using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class StudentSubject : BaseEntity
    {
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }


        public string SubjectId { get; set; }

        public virtual Subject Subject { get; set; }


        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}