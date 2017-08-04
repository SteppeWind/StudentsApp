using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.Contracts;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Class for work with history faculty
    /// </summary>
    public class HistoryFacultyService : BaseService<DeanFacultyDTO, DeanFaculty>, IHistoryFacultyService
    {
        public HistoryFacultyService(IUnitOfWork uow) : base(uow) { }

        public IEnumerable<DeanFacultyDTO> GetAll => Convert(DataBase.DeanFacultyRepository.GetAll);

        private DeanFaculty GetDeanFacultyIfExist(int id)
        {
            var result = DataBase.DeanFacultyRepository[id];

            if (result == null)
            {
                throw new ValidationException("Экзмепляр не найден");
            }

            return result;
        }

        /// <summary>
        /// Add branch in history
        /// </summary>
        /// <param name="entity"></param>
        public void Add(DeanFacultyDTO entity)//?*
        {
            var model = ReverseConvert(entity);

            DataBase.DeanFacultyRepository.Add(model);
            DataBase.Save();
        }

        public void FullRemove(int id)//?*
        {
            var model = GetDeanFacultyIfExist(id);

            DataBase.DeanFacultyRepository.FullRemove(model);
            DataBase.Save();
        }

        public DeanFacultyDTO Get(int id)
        {
            return Convert(GetDeanFacultyIfExist(id));
        }

        public void Remove(int id)//?*
        {
            FullRemove(id);
        }

        public void Update(DeanFacultyDTO entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get history individual faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<DeanFacultyDTO> GetHistory(int idFaculty)
        {
            return GetAll.Where(df => df.FacultyId == idFaculty);
        }
    }
}