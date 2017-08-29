using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class SubjectViewModel : BaseViewModel
    {
        public string SubjectName { get; set; }
        
        public string FacultyId { get; set; }

        public string FacultyName { get; set; }
    }
}