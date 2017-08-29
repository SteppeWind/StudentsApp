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
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }


        public string FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }


        public string PostTeacherId { get; set; }

        public virtual PostTeacher Post { get; set; }
    }
}
