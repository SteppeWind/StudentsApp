using AutoMapper;
using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Abstract class which provide base realisation to services
    /// </summary>
    /// <typeparam name="TEntityDTO"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseService<TEntityDTO, TEntity>
        where TEntityDTO : BaseDTO
        where TEntity : BaseEntity
    {
        protected IUnitOfWork DataBase { get; set; }

        public BaseService(IUnitOfWork uow)
        {
            DataBase = uow;
        }

        /// <summary>
        /// Convert collection entities
        /// </summary>
        /// <param name="baseEntities"></param>
        /// <returns></returns>
        protected IEnumerable<TEntityDTO> Convert(IEnumerable<TEntity> baseEntities)
        {
            return baseEntities.Select(e => Convert(e));
        }

        /// <summary>
        /// Convert entity from TEntity in DB to TEntityDTO
        /// </summary>
        /// <param name="entity">Base entity</param>
        /// <returns></returns>
        protected virtual TEntityDTO Convert(TEntity entity)
        {
            //use automapper from convert 
            Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TEntityDTO>());
            return Mapper.Map<TEntity, TEntityDTO>(entity);
        }

        protected T1 Map<T1, T2>(T2 t2)
            where T1 : class
            where T2 : class
        {
            Mapper.Initialize(cfg => cfg.CreateMap<T2, T1>());
            return Mapper.Map<T2, T1>(t2);
        }

        /// <summary>
        /// Reverese convert collection TEntityDTO
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected IEnumerable<TEntity> ReverseConvert(IEnumerable<TEntityDTO> entities)
        {
            return entities.Select(e => ReverseConvert(e));
        }

        /// <summary>
        /// Convert from TEntityDTO to TEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TEntity ReverseConvert(TEntityDTO entity)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TEntityDTO, TEntity>());
            return Mapper.Map<TEntityDTO, TEntity>(entity);
        }

        /// <summary>
        /// Universal convert for different entities, almost:)
        /// </summary>
        /// <typeparam name="Entity">Entity from DB</typeparam>
        /// <typeparam name="EntityDTO">Entity from DTO</typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual EntityDTO UniversalConvert<Entity, EntityDTO>(Entity entity)
            where Entity : BaseEntity
            where EntityDTO : BaseDTO
        {
            //это не совсем универсальное конвертирование, делал давно, сейчас понял, что было лишним
            Mapper.Initialize(cfg => cfg.CreateMap<Entity, EntityDTO>());
            return Mapper.Map<Entity, EntityDTO>(entity);
        }

        //продолжение потока сумашествия 
        protected virtual Entity UniversalReverseConvert<Entity, EntityDTO>(EntityDTO entity)
            where Entity : BaseEntity
            where EntityDTO : BaseDTO
        {
            Mapper.Initialize(cfg => cfg.CreateMap<EntityDTO, Entity>());
            return Mapper.Map<EntityDTO, Entity>(entity);
        }


        protected IEnumerable<EntityDTO> UniversalConvert<Entity, EntityDTO>(IEnumerable<Entity> entities)
            where Entity : BaseEntity
            where EntityDTO : BaseDTO
        {
            return entities.Select(e => UniversalConvert<Entity, EntityDTO>(e));
        }

        protected IEnumerable<Entity> UniversalReverseConvert<Entity, EntityDTO>(IEnumerable<EntityDTO> entities)
            where Entity : BaseEntity
            where EntityDTO : BaseDTO
        {
            return entities.Select(e => UniversalReverseConvert<Entity, EntityDTO>(e));
        }
    }
}