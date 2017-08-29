using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.WEB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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
                if (IsCorrectDean(group.FacultyId))
                {
                    var groupDTO = BaseViewModel.UniversalReverseConvert<GroupViewModel, GroupDTO>(group);
                    var result = await GroupService.AddAsync(groupDTO);
                    if (result.Succedeed)
                    {
                        TempData["message"] = result.Message;
                    }
                    else
                    {
                        TempData["errorMessage"] = result.Message;
                    }
                }
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(GroupViewModel group, string returnUrl)
        {
            if (IsCorrectDean(group.FacultyId))
            {
                var groupDTO = BaseViewModel.UniversalReverseConvert<GroupViewModel, GroupDTO>(group);
                var result = await GroupService.UpdateAsync(groupDTO);
                if (result.Succedeed)
                {
                    TempData["message"] = result.Message;
                }
                else
                {
                    TempData["errorMessage"] = result.Message;
                }
            }

            return Redirect(returnUrl);
        }

        private bool IsCorrectDean(string idfaculty)
        {
            var deanDTO = DeanService.Get(User.Identity.GetUserId());

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