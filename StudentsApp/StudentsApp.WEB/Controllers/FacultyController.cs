using PagedList;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.WEB.Models.Entities;
using StudentsApp.WEB.Models.Entities.ComplexEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    public class FacultyController : Controller
    {
        IFacultyService FacultyService;
        IHistoryFacultyService HistoryFacultyService;
        IDeanService DeanService;
        IGroupService GroupService;

        public FacultyController(
            IFacultyService facultyService,
            IHistoryFacultyService historyFacultyService,
            IDeanService deanService,
            IGroupService groupService)
        {
            FacultyService = facultyService;
            HistoryFacultyService = historyFacultyService;
            DeanService = deanService;
            GroupService = groupService;
        }

        int pageSize = 10;

        // GET: Faculty
        public ActionResult Index(int? page = 1)
        {
            int pageNumber = (page ?? 1);
            return View(BaseViewModel.UniversalConvert<FacultyDTO, FacultyViewModel>(FacultyService.GetAll).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(string id = "")
        {
            ComplexFaculty facultyVM = new ComplexFaculty();

            var facultyDTO = FacultyService.Get(id);
            var historiesFacultiesDTO = HistoryFacultyService.GetHistory(id).ToList();
            var deansDTO = DeanService.GetDeansInFaculty(id);
            var groupsDTO = GroupService.GetGroupsInFaculty(id).ToList();            

            //convert ComplexFaculty to finded FacultyDTO
            facultyVM = BaseViewModel.UniversalConvert<FacultyDTO, ComplexFaculty>(facultyDTO);
            //convert info about HistoryFaculties to finded historiesFacultiesDTO by Id
            facultyVM.ListHistoryFaculties = BaseViewModel.UniversalConvert<DeanFacultyDTO, HistoryFacultyViewModel>(historiesFacultiesDTO).ToList();
            facultyVM.ListGroups = BaseViewModel.UniversalConvert<GroupDTO, GroupViewModel>(groupsDTO).ToList();

            //find profile deans who manage current faculty             
            foreach (var item in facultyVM.ListHistoryFaculties)
            {
                var dean = BaseViewModel
                    .UniversalConvert<DeanDTO, PersonViewModel>
                    (deansDTO.FirstOrDefault(d => d.ListIdDeanFaculties.Contains(item.Id)));
                item.Dean = dean;
                if (item.EndManage == null)
                {
                    facultyVM.Dean = dean;
                }
            }

            return View(facultyVM);
        }

    }
}