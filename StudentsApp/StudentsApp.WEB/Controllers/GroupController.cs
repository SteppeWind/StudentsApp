using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.WEB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Controllers
{
    [Authorize(Roles = "dean, admin")]
    public class GroupController : Controller
    {
        private IGroupService GroupService;
        private IFacultyService FacultyService;
        private IDeanService DeanService;


        public GroupController(
            IGroupService groupService,
            IDeanService deanService,
            IFacultyService facultyService)
        {
            FacultyService = facultyService;
            DeanService = deanService;
            GroupService = groupService;
        }

        [HttpPost]
        public async Task<ActionResult> Create(GroupViewModel group, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await IsCorrectDean(group.FacultyId))
                    {
                        var groupDTO = BaseViewModel.UniversalReverseConvert<GroupViewModel, GroupDTO>(group);
                        GroupService.Add(groupDTO);
                        TempData["message"] = "Изменения были сохранены";
                    }
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                }
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(GroupViewModel group, string returnUrl)
        {
            try
            {
                if (await IsCorrectDean(group.FacultyId))
                {
                    var groupDTO = BaseViewModel.UniversalReverseConvert<GroupViewModel, GroupDTO>(group);
                    GroupService.Update(groupDTO);
                    TempData["message"] = "Изменения были сохранены";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return Redirect(returnUrl);
        }

        private async Task<bool> IsCorrectDean(int idfaculty)
        {
            var deanDTO = await DeanService.GetByEmailAsync(User.Identity.Name);

            if (deanDTO.FacultyId == idfaculty)
            {
                return true;
            }
            else
            {
                TempData["errorMessage"] = "У вас не хватает прав на изменение";
                return false;
            }
        }
    }
}