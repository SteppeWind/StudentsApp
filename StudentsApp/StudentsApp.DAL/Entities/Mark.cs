using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    /// <summary>
    /// Inforamation about subjects which passed by students
    /// </summary>
    public class Mark : BaseEntity
    {
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }
        

        public string SubjectId { get; set; }

        public virtual Subject Subject { get; set; }


        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
        

        public SubjectType Type { get; set; }

        public DateTime DateSubjectPassing { get; set; }

        public byte SemesterNumber { get; set; }
    }
}
