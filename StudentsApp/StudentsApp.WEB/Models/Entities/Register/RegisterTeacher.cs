using StudentsApp.WEB.Models.Entities.Edit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities.Register
{
    public class RegisterTeacher : RegisterVIewModel
    {
        public string FacultyId { get; set; }

        [Display(Name = "Степень преподавателя")]
        public SelectList Posts { get; set; }

        /// <summary>
        /// This save ALL subjects in university
        /// </summary>
        public List<SubjectViewModel> Subjects { get; set; }


        public IEnumerable<IGrouping<string, SubjectViewModel>> GroupSubjects => Subjects.GroupBy(s => s.FacultyName);

        public List<string> SelectedIdPosts { get; set; }       

        public List<string> SelectedIdSubjects { get; set; }
        
        public RegisterTeacher()
        {

            SelectedIdPosts = new List<string>();
            SelectedIdSubjects = new List<string>();           
        }
    }
}