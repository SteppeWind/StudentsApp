using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class TeacherFaculty : BaseEntity
    {
        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }

        [ForeignKey("Post")]
        public int PostTeacherId { get; set; }

        public virtual PostTeacher Post { get; set; }
    }
}
