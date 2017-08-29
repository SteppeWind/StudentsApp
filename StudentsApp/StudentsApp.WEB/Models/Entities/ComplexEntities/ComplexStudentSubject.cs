using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexStudentSubject : StudentSubjectViewModel
    {
        public StudentViewModel Student { get; set; }
    }
}