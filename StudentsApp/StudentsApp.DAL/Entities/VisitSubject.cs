﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class VisitSubject : BaseEntity
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }


        public int SubjectId { get; set; }

        public Subject Subject { get; set; }


        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; }
    }
}