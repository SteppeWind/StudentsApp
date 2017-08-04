using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexTestMark : ComplexMark
    {
        [DisplayName("Статус")]
        public bool IsPassed { get; set; }


        public ComplexTestMark()
        {
            Type = SubjectTypeViewModel.Test;
        }
    }
}