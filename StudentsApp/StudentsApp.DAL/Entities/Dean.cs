using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    [Table("Deans")]
    public class Dean : Person
    {
        public virtual ICollection<DeanFaculty> ListDeanFaculties { get; set; }

        public Dean()
        {
            ListDeanFaculties = new List<DeanFaculty>();
        }
    }
}