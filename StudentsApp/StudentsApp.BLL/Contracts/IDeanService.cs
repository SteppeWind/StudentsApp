﻿using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IDeanService : IPersonService<DeanDTO>, IBaseService<DeanDTO>
    {
        void RemoveFaculty(int idDean, int idFaculty);
        void AddFaculty(int idDean, int idFaculty);        
    }
}