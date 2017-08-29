using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class TeacherFacultyVIewModel : BaseViewModel
    {        
        public string FacultyId { get; set; }
     
        public string FacultyName { get; set; }

        public string PostTeacherId { get; set; }
        
        public string NamePostTeacher { get; set; }
    }
}