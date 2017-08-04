using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Contracts;
using AutoMapper;
using StudentsApp.DAL.Entities;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    public class GroupService : BaseService<GroupDTO, Group>, IGroupService
    {
        public GroupService(IUnitOfWork uow) : base(uow) { }

        private Group GetGroupIfExist(int id)
        {
            var group = DataBase.GroupRepository[id];

            if (group == null)
            {
                throw new ValidationException("Группа не найдена");
            }

            return group;
        }


        private Student GetStudentIfExist(int id)
        {
            var student = DataBase.StudentRepository[id];

            if (student == null)
            {
                throw new ValidationException("Студент не найден");
            }

            return student;
        }

        private Student GetStudentIfExist(string email)
        {
            var student = DataBase.StudentRepository.GetByEmail(email);

            if (student == null)
            {
                throw new ValidationException("Студент не найден");
            }

            return student;
        }

        protected override GroupDTO Convert(Group entity)
        {
            var result = base.Convert(entity);

            result.FacultyName = entity.Faculty.FacultyName;
            //save id`s students in current group
            result.ListIdStudents = entity.ListStudents.Select(g => g.Id);

            return result;
        }


        /// <summary>
        /// Get all groups
        /// </summary>
        public IEnumerable<GroupDTO> GetAll
        {
            get
            {
                return Convert(DataBase.GroupRepository.GetAll);
            }
        }

        /// <summary>
        /// Add group in DB
        /// </summary>
        /// <param name="entity">Added group</param>
        public void Add(GroupDTO entity)
        {
            var group = ReverseConvert(entity);//convert group

            //check if name group is exist in DB
            if (DataBase.GroupRepository.Find(g => g.GroupName.Equals(entity.GroupName)).Any())
            {
                throw new ValidationException($"Группа с названием {entity.GroupName} уже сущесвует");
            }

            DataBase.GroupRepository.Add(group);
            DataBase.Save();
        }

        /// <summary>
        /// Add student to group
        /// </summary>
        /// <param name="idGroup"></param>
        /// <param name="idStudent"></param>
        public void AddStudentToGroup(int idGroup, int idStudent)
        {
            var group = GetGroupIfExist(idGroup);
            var student = GetStudentIfExist(idStudent);

            DataBase.GroupRepository.AddStudentToGroup(group, student.Id);
            DataBase.Save();
        }

        /// <summary>
        /// Full delete group from DB
        /// </summary>
        /// <param name="id">Id group</param>
        public void FullRemove(int id)
        {
            var group = GetGroupIfExist(id);

            DataBase.StudentRepository.FullRemove(group.ListStudents);
            DataBase.GroupRepository.FullRemove(group);
            DataBase.Save();
        }

        /// <summary>
        /// Get group by id
        /// </summary>
        /// <param name="id">Id group</param>
        /// <returns>If group isnot exist throw ValidationEcxception, else return convert group</returns>
        public GroupDTO Get(int id)
        {
            return Convert(GetGroupIfExist(id));
        }

        public void Remove(int id)
        {
            var group = GetGroupIfExist(id);

            DataBase.GroupRepository.Remove(group);
            DataBase.Save();
        }

        /// <summary>
        /// Remove student from group
        /// </summary>
        /// <param name="idGroup">Id group</param>
        /// <param name="idStudent">Id student</param>
        public void RemoveStudentFromGroup(int idGroup, int idStudent)
        {
            var group = GetGroupIfExist(idGroup);//find group
            var student = GetStudentIfExist(idStudent);//fint student

            DataBase.GroupRepository.RemoveStudentFromGroup(group, student.Id);
            DataBase.Save();
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="entity">Any group</param>
        public void Update(GroupDTO entity)//*
        {
            var check = DataBase.GroupRepository.Find(g => g.GroupName.Equals(entity.GroupName)).Any();//find groups with same name
            if (check)//if find
            {
                throw new ValidationException($"Группа с названием {entity.GroupName} уже существует");
            }

            var newGroup = ReverseConvert(entity);//convert DTO to entity DB
            DataBase.GroupRepository.Update(newGroup);
            DataBase.Save();
        }

        /// <summary>
        /// Add student to group by email
        /// </summary>
        /// <param name="idGroup">Group id</param>
        /// <param name="email">Email student</param>
        public void AddStudentToGroup(int idGroup, string email)
        {
            var student = GetStudentIfExist(email);//find student by email
            var group = GetGroupIfExist(idGroup);//find group by id

            DataBase.GroupRepository.AddStudentToGroup(idGroup, email);//add student to group
            DataBase.Save();
        }

        public IEnumerable<GroupDTO> GetGroupsInFaculty(int idFaculty)
        {
            return GetAll.Where(g => g.FacultyId == idFaculty);
        }

        public IEnumerable<GroupDTO> GetStudentGroups(int idStudent)
        {
            return GetAll.Where(g => g.ListIdStudents.Contains(idStudent));
        }
    }
}