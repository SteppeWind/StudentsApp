using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class StudentViewModel : PersonViewModel
    {           
        public double AverageMark { get; set; }
    }
}