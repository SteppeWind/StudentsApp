using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class HistoryFacultyViewModel : BaseViewModel
    {
        public string FacultyId { get; set; }

        public PersonViewModel Dean { get; set; }

        public DateTime StartManage { get; set; }

        public DateTime? EndManage { get; set; }
    }
}