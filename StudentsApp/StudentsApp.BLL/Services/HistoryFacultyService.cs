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

        public int Count => DataBase.DeanFacultyRepository.Count;

        private DeanFaculty GetDeanFacultyIfExist(string id)
        {
            var result = DataBase.DeanFacultyRepository[id];

            if (result == null)
            {
                throw new ValidationException("Экзмепляр не найден");
            }

            return result;
        }

        /// <summary>
        /// Add branch in history faculty
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationDetails> AddAsync(DeanFacultyDTO entity)//?*
        {
            OperationDetails answer = null;

            try
            {
                var model = ReverseConvert(entity);
                DataBase.DeanFacultyRepository.Add(model);
                await DataBase.SaveAsync();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }
            
            return answer;
        }

        public OperationDetails FullRemove(string id)//?*
        {
            OperationDetails answer = null;

            try
            {
                var model = GetDeanFacultyIfExist(id);

                DataBase.DeanFacultyRepository.FullRemove(model);
                DataBase.Save();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public DeanFacultyDTO Get(string id)
        {
            return Convert(GetDeanFacultyIfExist(id));
        }
        

        public OperationDetails Remove(string id)//?*
        {
            OperationDetails answer = null;

            try
            {
                var model = GetDeanFacultyIfExist(id);

                DataBase.DeanFacultyRepository.Remove(model);
                DataBase.Save();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public Task<OperationDetails> UpdateAsync(DeanFacultyDTO entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get history individual faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<DeanFacultyDTO> GetHistory(string idFaculty)
        {
            return GetAll.Where(df => df.FacultyId == idFaculty);
        }
    }
}