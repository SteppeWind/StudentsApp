using Moq;
using NUnit.Framework;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Services;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using StudentsApp.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.UnitTests.Services
{
    [TestFixture]
    public class DeanServiceTests
    {
        [Test]
        public void GetAll_InitializedListDeansDTO_ReturnListDeansDTOEqualsInitializedListDeansDTO()
        {
            //! Arrange
            var deansDTOMock = new Mock<IDeanService>();
            deansDTOMock.Setup(serv => serv.GetAll).Returns(GetTestDeansDTO());
            
            //! Act
            var result = GetTestDeansDTO();

            //! Assert
            Assert.AreSame(deansDTOMock.Object.GetAll, result);
        }

        [Test]
        public void Remove_InitializedIdDeanDTO_Returt_ReturnOperationDetailsWithSuccedeedEqualsTrue()
        {
            //! Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var deanRepositoryMock = new Mock<AbstractPersonRepository<Dean>>();
            deanRepositoryMock.Setup(repos => repos.GetAll).Returns(GetTestDeans());
            unitOfWorkMock.Setup(uow => uow.DeanRepository).Returns(deanRepositoryMock.Object);
            var deanService = new DeanService(unitOfWorkMock.Object);
            string id = "1";

            //! Act
            var result = deanService.Remove(id);

            //! Assert
            Assert.AreEqual(true, result.Succedeed);
        }        

        [Test]
        public void UpdateAsync_InitializedDeanDTO_ReturnOperationDetailsWithSuccedeedEqualsTrue()
        {
            //! Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var deanRepositoryMock = new Mock<AbstractPersonRepository<Dean>>();
            deanRepositoryMock.Setup(repos => repos.GetAll).Returns(GetTestDeans());
            unitOfWorkMock.Setup(uow => uow.DeanRepository).Returns(deanRepositoryMock.Object);
            var deanService = new DeanService(unitOfWorkMock.Object);
            
            //! Act
           
        }

        //[Test]
        //public async Task AddAsync_InititalizedDeanDTO_ReturnOperationDetailsWithSuccedeedEqualsTrue()
        //{
        //    //! Arrange
        //    var unitOfWorkMock = new Mock<IUnitOfWork>();
        //    var deanRepositoryMock = new Mock<AbstractPersonRepository<Dean>>();
        //    //var profileManagerMock = new ApplicationProfileManager()
        //    unitOfWorkMock.Setup(service => service.ProfileManager.Users).Returns(GetTestProfiles().AsQueryable());
        //    unitOfWorkMock.Setup(service => service.DeanRepository.GetAll).Returns(GetTestDeans());            
        //    var deanService = new DeanService(unitOfWorkMock.Object);
        //    var deanDTO = new DeanDTO() { Name = "Сергей", Surname = "Шнуров", MiddleName = "Владимирович", Email = "Shnur@mail.ru" };

        //    //! Act
        //    var result = await deanService.AddAsync(deanDTO);

        //    //! Assert
        //    Assert.AreEqual(true, result.Succedeed);
        //}


        private List<Profile> GetTestProfiles()
        {
            return new List<Profile>()
            {
                new Profile() {Id = "1", Name = "Валерий", MiddleName="Иванович", Surname="Хабаров",  Email="Habar@mail.ru" },
                new Profile() {Id = "2", Name="Иван", MiddleName="Андреевич",Surname="Ургант", Email="Urgant@mail.ru" }
            };
        }

        private List<DeanDTO> GetTestDeansDTO()
        {
            return GetTestDeans().Select(d => new DeanDTO()
            {
                Id = d.Id,
                Name = d.Profile.Name,
                Surname = d.Profile.Surname,
                MiddleName = d.Profile.MiddleName,
                Email = d.Profile.Email
            }).ToList();
        }

        private List<Dean> GetTestDeans()
        {
            return GetTestProfiles().Select(p => new Dean()
            {
                Id = p.Id,
                Profile = p
            }).ToList();            
        }

    }
}