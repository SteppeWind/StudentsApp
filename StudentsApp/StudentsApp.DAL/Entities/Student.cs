using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    [Table("Students")]
    public class Student : Person
    {
        public virtual ICollection<Mark> ListMarks { get; set; }

        public virtual ICollection<VisitSubject> ListVisitSubjects { get; set; }        

        public virtual ICollection<Group> ListGroups { get; set; }

        [NotMapped]
        public double AverageMark
        {
            get
            {
                List<ExamMark> examMarks = new List<ExamMark>();

                ListMarks.ToList().ForEach(m =>
                {
                    if (m is ExamMark exam && exam.Mark > 2)
                        examMarks.Add(exam);
                });

                return examMarks.Count > 0 ? examMarks.Average(em => em.Mark) : 0;
            }
        }

        public Student()
        {
            ListMarks = new List<Mark>();
            ListVisitSubjects = new List<VisitSubject>();
            ListGroups = new List<Group>();
        }
    }
}
