using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    [Table("TestMarks")]
    public class TestMark : Mark
    {
        public bool IsPassed { get; set; }

        public TestMark()
        {
            Type = SubjectType.Test;
        }
    }
}