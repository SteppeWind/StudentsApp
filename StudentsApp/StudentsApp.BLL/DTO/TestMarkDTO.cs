using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class TestMarkDTO : MarkDTO
    {
        public bool IsPassed { get; set; }

        public TestMarkDTO()
        {
            Type = SubjectTypeDTO.Test;
        }
    }
}