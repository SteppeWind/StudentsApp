using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class BaseDTO
    {
        public int Id { get; set; }

        public bool IsDelete { get; set; }

        public BaseDTO()
        {
            IsDelete = false;
        }
    }
}