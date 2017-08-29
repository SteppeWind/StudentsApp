using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface ITeacherFacultyService : IBaseService<TeacherFacultyDTO>
    {
        IEnumerable<TeacherFacultyDTO> GetTeacherPosts(string teacherId);
        Task<OperationDetails> AddByTeacherEmail(string email, TeacherFacultyDTO entity);
    }
}