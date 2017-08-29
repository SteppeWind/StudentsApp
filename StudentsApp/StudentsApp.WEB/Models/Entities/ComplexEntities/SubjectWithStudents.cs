using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class SubjectWithStudents : SubjectViewModel
    {
        public IEnumerable<StudentViewModel> Students { get; set; }

        public SubjectWithStudents()
        {
            Students = new List<StudentViewModel>();
        }
    }
}