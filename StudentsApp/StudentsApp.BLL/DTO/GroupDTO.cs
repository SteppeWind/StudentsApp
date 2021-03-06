﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class GroupDTO : BaseDTO
    {
        public string GroupName { get; set; }

        public string FacultyId { get; set; }        

        public string FacultyName { get; set; }

        public IEnumerable<string> ListIdStudents { get; set; }

        public GroupDTO()
        {
            ListIdStudents = new List<string>();
        }
    }
}