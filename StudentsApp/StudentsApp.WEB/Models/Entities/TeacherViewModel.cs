using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class TeacherViewModel : PersonViewModel
    {
        public int CountStudents { get; set; }
    }
}