using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class DeanFacultyDTO : BaseDTO
    {
        public int FacultyId { get; set; }
        
        public int DeanId { get; set; }
        
        public DateTime StartManage { get; set; }

        public DateTime? EndManage { get; set; }
    }
}