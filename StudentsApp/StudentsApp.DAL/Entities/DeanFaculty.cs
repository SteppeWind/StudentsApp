using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class DeanFaculty : BaseEntity
    {
        public string FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }

        
        public string DeanId { get; set; }

        public virtual Dean Dean { get; set; }


        public DateTime StartManage { get; set; }

        public DateTime? EndManage { get; set; }
    }
}