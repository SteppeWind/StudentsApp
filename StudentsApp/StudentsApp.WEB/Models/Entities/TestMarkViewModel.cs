using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class TestMarkViewModel : MarkViewModel
    {
        public bool IsPassed { get; set; }
    }
}