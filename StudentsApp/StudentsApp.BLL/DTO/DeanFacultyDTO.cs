using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class DeanFacultyDTO : BaseDTO
    {
        public string FacultyId { get; set; }
        
        public string DeanId { get; set; }
        
        public DateTime StartManage { get; set; }

        public DateTime? EndManage { get; set; }
    }
}