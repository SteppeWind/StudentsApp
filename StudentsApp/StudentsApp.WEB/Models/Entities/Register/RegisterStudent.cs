using StudentsApp.WEB.Models.Entities.ComplexEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities.Register
{
    public class RegisterStudent : RegisterVIewModel
    {        
        public string FacultyId { get; set; }

        [Display(Name = "Список групп")]
        public IList<GroupViewModel> Groups { get; set; }
        
        [Display(Name = "Список предметов")]
        public IList<SubjectViewModel> Subjects { get; set; }
        
        public List<SubjectWithTeachers> SubjectsWithTeachers { get; set; }


        public IEnumerable<IGrouping<string, SubjectWithTeachers>> GropedSubjectsWithTeachers => SubjectsWithTeachers.GroupBy(st => st.FacultyName);


        public string IdGroup { get; set; }
        
        public List<string> SelectedIdTeachers { get; set; }
      
        public List<string> SelectedIdSubjects { get; set; }

        public RegisterStudent()
        {
            SubjectsWithTeachers = SubjectsWithTeachers ?? new List<SubjectWithTeachers>();

            if (Groups == null)
                Groups = new List<GroupViewModel>();

            if (Subjects == null)
                Subjects = new List<SubjectViewModel>();

            if (SelectedIdSubjects == null)
                SelectedIdSubjects = new List<string>();
        }
    }
}