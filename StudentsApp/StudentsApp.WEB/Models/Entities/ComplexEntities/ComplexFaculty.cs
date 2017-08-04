using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexFaculty : FacultyViewModel
    {       
        public PersonViewModel Dean { get; set; }


        public List<HistoryFacultyViewModel> ListHistoryFaculties { get; set; }

        public List<GroupViewModel> ListGroups { get; set; }

        public ComplexFaculty()
        {
            ListHistoryFaculties = new List<HistoryFacultyViewModel>();
            ListGroups = new List<GroupViewModel>();
        }
    }
}