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


        private bool IsStudentExitsInGroup(string studentId, string groupId)
        {
            return DataBase.StudentGroupRepository
                    .FindFirst(sg => sg.StudentId == studentId && sg.GroupId == groupId) != null;
        }

        private bool IsGroupExistByName(string groupName)
        {
            return DataBase.GroupRepository.FindFirst(g => g.GroupName.Equals(groupName)) != null;
        }

        private Group GetGroupIfExist(string id)
        {
            var group = DataBase.GroupRepository[id];

            if (group == null)
            {
                throw new ValidationException("Группа не найдена");
            }

            return group;
        }


        private Student GetStudentIfExistById(string id)
        {
            var student = DataBase.StudentRepository[id];

            if (student == null)
            {
                throw new ValidationException("Студент не найден");
            }

            return student;
        }

        private Student GetStudentIfExistByEmail(string email)
        {
            var student = DataBase.StudentRepository.GetByEmail(email);

            if (student == null)
            {
                throw new ValidationException($"Студент по email {email} не найден");
            }

            return student;
        }

        protected override GroupDTO Convert(Group entity)
        {
            var result = base.Convert(entity);

            result.FacultyName = entity.Faculty.FacultyName;
            //save id`s students in current group
            result.ListIdStudents = entity.ListStudents.Select(g => g.StudentId);

            return result;
        }


        private OperationDetails AddStudentToGroup(Student student, Group group)
        {
            OperationDetails answer = null;

            //check student, if he already exists in this group       
            if (IsStudentExitsInGroup(student.Id, group.Id))
            {
                answer = new OperationDetails(false, $"Студент {student.Profile.ToString()} уже состоит в группе {group.GroupName}");
            }
            else
            {
                DataBase.StudentGroupRepository.Add(new StudentGroup()
                {
                    GroupId = group.Id,
                    StudentId = student.Id
                });
                DataBase.Save();
                answer = new OperationDetails(true, $"Студент {student.Profile.ToString()} успешно добавлен в группу {group.GroupName}");
            }

            return answer;
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

        public int Count => DataBase.GroupRepository.Count;

        /// <summary>
        /// Add group in DB
        /// </summary>
        /// <param name="entity">Added group</param>
        /// <returns></returns>
        public async Task<OperationDetails> AddAsync(GroupDTO entity)
        {
            OperationDetails answer = null;

            var group = ReverseConvert(entity);//convert group
            
            //check if name group is exist in DB
            if (IsGroupExistByName(entity.GroupName))
            {
                answer = new OperationDetails(false, $"Группа с названием '{entity.GroupName}' уже сущесвует");
            }
            else
            {
                group.Id = BaseEntity.GenerateId;
                DataBase.GroupRepository.Add(group);
                await DataBase.SaveAsync();
                answer = new OperationDetails(true, $"Группа '{entity.GroupName}' успешно создана");
            }

            return answer;
        }

        /// <summary>
        /// Add student to group
        /// </summary>
        /// <param name="idGroup"></param>
        /// <param name="idStudent"></param>
        /// <returns></returns>
        public OperationDetails AddStudentToGroupById(string idGroup, string idStudent)
        {
            OperationDetails answer = null;

            try
            {
                var group = GetGroupIfExist(idGroup);
                var student = GetStudentIfExistById(idStudent);

                answer = AddStudentToGroup(student, group);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Full delete group from DB
        /// </summary>
        /// <param name="id">Id group</param>
        /// <returns></returns>
        public OperationDetails FullRemove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var group = GetGroupIfExist(id);

                DataBase.StudentGroupRepository.FullRemove(group.ListStudents);
                DataBase.GroupRepository.FullRemove(group);
                DataBase.Save();
                answer = new OperationDetails(true, $"Группа {group.GroupName} полностью удален из базы");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Get group by id
        /// </summary>
        /// <param name="id">Id group</param>
        /// <returns>If group isnot exist throw ValidationEcxception, else return convert group</returns>
        public GroupDTO Get(string id)
        {
            return Convert(GetGroupIfExist(id));
        }


        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var group = GetGroupIfExist(id);

                DataBase.GroupRepository.Remove(group);
                DataBase.Save();
                answer = new OperationDetails(true, $"Группа {group.GroupName} помечена как 'Удалён'");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Remove student from group
        /// </summary>
        /// <param name="idGroup">Id group</param>
        /// <param name="idStudent">Id student</param>
        /// <returns></returns>
        public OperationDetails RemoveStudentFromGroupById(string idGroup, string idStudent, bool isFullRemove = false)
        {
            OperationDetails answer = null;

            try
            {
                var group = GetGroupIfExist(idGroup);//find group
                var student = GetStudentIfExistById(idStudent);//fint student
                //ищем запись, в которой хранится информация о студенте в группе
                var deletedObject = DataBase.StudentGroupRepository
                        .FindFirst(sg => sg.StudentId == idStudent && sg.GroupId == idGroup);

                if (isFullRemove)
                {
                    DataBase.StudentGroupRepository.FullRemove(deletedObject);
                }
                else
                {
                    DataBase.StudentGroupRepository.Remove(deletedObject);
                }

                DataBase.Save();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="entity">Any group</param>
        /// <returns></returns>
        public async Task<OperationDetails> UpdateAsync(GroupDTO entity)//*
        {
            OperationDetails answer = null;

            if (IsGroupExistByName(entity.GroupName))//if find
            {
                answer = new OperationDetails(false, $"Группа с названием {entity.GroupName} уже существует");
            }
            else
            {
                var newGroup = ReverseConvert(entity);//convert DTO to entity DB
                DataBase.GroupRepository.Update(newGroup);
                await DataBase.SaveAsync();
                answer = new OperationDetails(true, "Данные сохранены");
            }

            return answer;
        }

        /// <summary>
        /// Add student to group by email
        /// </summary>
        /// <param name="idGroup">Group id</param>
        /// <param name="email">Email student</param>
        /// <returns></returns>
        public OperationDetails AddStudentToGroupByEmail(string idGroup, string email)
        {
            OperationDetails answer = null;
            try
            {
                var student = GetStudentIfExistByEmail(email);//find student by email
                var group = GetGroupIfExist(idGroup);//find group by id

                answer = AddStudentToGroup(student, group);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }


            return answer;
        }

        public IEnumerable<GroupDTO> GetGroupsInFaculty(string idFaculty)
        {
            var result = Convert(DataBase.GroupRepository.Find(g => g.FacultyId == idFaculty));
            return result;
        }

        public IEnumerable<GroupDTO> GetStudentGroups(string idStudent)
        {
            var result = Convert(DataBase.GroupRepository.Find(g => g.ListStudents.Select(s => s.StudentId).Contains(idStudent)));
            return result;
        }
    }
}