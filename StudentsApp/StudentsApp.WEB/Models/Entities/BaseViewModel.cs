using AutoMapper;
using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities
{
    public class BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public bool IsDelete { get; set; }

        public static EntityVM UniversalConvert<EntityDTO, EntityVM>(EntityDTO entity)
          where EntityDTO : BaseDTO
          where EntityVM : BaseViewModel
        {
            Mapper.Initialize(cfg => cfg.CreateMap<EntityDTO, EntityVM>());
            return Mapper.Map<EntityDTO, EntityVM>(entity);
        }

        public static IEnumerable<EntityVM> UniversalConvert<EntityDTO, EntityVM>(IEnumerable<EntityDTO> entities)
            where EntityDTO : BaseDTO
            where EntityVM : BaseViewModel
        {
            return entities.Select(e => UniversalConvert<EntityDTO, EntityVM>(e));
        }

        public static EntityDTO UniversalReverseConvert<EntityVM, EntityDTO>(EntityVM entity)
          where EntityDTO : BaseDTO
          where EntityVM : BaseViewModel
        {
            Mapper.Initialize(cfg => cfg.CreateMap<EntityVM, EntityDTO>());
            return Mapper.Map<EntityVM, EntityDTO>(entity);
        }

    }
}