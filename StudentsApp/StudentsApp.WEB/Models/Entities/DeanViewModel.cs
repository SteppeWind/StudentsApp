using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class DeanViewModel : PersonViewModel
    {
        public int FacultyId { get; set; }
    }
}