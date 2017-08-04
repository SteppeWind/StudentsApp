using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IGroupService : IBaseService<GroupDTO>
    {
        void AddStudentToGroup(int idGroup, int idStudent);
        void AddStudentToGroup(int idGroup, string email);
        void RemoveStudentFromGroup(int idGroup, int idStudent);
        IEnumerable<GroupDTO> GetGroupsInFaculty(int idFaculty);
        IEnumerable<GroupDTO> GetStudentGroups(int idStudent);
    }
}