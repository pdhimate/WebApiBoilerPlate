using Api.BusinessEntities;
using Api.BusinessEntities.AccountController;
using Api.BusinessEntities.Common;
using Api.BusinessEntities.PostsController;
using Api.BusinessEntities.RefreshTokenController;
using Api.BusinessEntities.UserController;
using Api.Data.Access;
using Api.Data.Access.Repositories;
using Api.Data.CosmosDb.Models;
using Api.Data.Models;
using Api.Data.Models.Security;
using AutoMapper;
using System;
using System.Linq.Expressions;

namespace Api.BusinessService
{
    public abstract class BaseService
    {
        #region Protected properties

        protected readonly IUnitOfWork UnitOfWork;

        #endregion

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            InitializeAutoMapper();
        }

        #region Private methods

        /// <summary>
        /// Initializes the <see cref="AutoMapper"/> with the mappings for each DataModel and Dto objects.
        /// Note: Update this mapping every time a new Dto/Model is added.
        /// </summary>
        private void InitializeAutoMapper()
        {
            Mapper.Initialize(config =>
            {
                // SQLserver models
                config.CreateMap<AppUser, RegisterUserReq>(MemberList.Destination);
                config.CreateMap<AppUser, ProfileDto>(MemberList.Destination);
                config.CreateMap<AppUser, UserResDto>(MemberList.Destination);
                config.CreateMap<Page<AppUser>, PagedRes<UserResDto>>(MemberList.Destination);

                config.CreateMap<RefreshToken, RefreshTokenRes>(MemberList.Destination);
                config.CreateMap<Page<RefreshToken>, PagedRes<RefreshTokenRes>>(MemberList.Destination);

                // Cosmos Db models
                config.CreateMap<CreateTextPostReq, TextPost>(MemberList.Destination);
                config.CreateMap<TextPost, TextPostRes>(MemberList.Destination);

            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Appends the warning messages to the <see cref="IWarningResponse.WarningMessage"/> property.
        /// </summary>
        /// <param name="responseDto"></param>
        /// <param name="warningMessages"></param>
        public virtual void AddWarnings(IWarningResponse responseDto, params string[] warningMessages)
        {
            responseDto.WarningMessage += string.Join(Environment.NewLine, warningMessages);
        }

        /// <summary>
        /// Saves all the pending changes in the UnitOfWork
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return UnitOfWork.Save();
        }

        #endregion

        #region Protected services

        protected Page<T> GetPage<T, TId>(int pageNumber,
                                          int pageSize,
                                          IGenericRepository<T, TId> repository,
                                          Expression<Func<T, TId>> idSelector)
            where T : class
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Invalid pageNumber: {pageNumber} and pageSize: {pageSize}. They must be greater than 0.");
            }

            return repository.GetByPage(pageNumber, pageSize, idSelector);
        }

        protected T GetById<T, TId>(TId id,
                                   IGenericRepository<T, TId> repository)
            where T : class
        {
            ValidateId(id);
            var entity = repository.GetById(id);
            if (entity == null)
            {
                throw new BusinnessException($"Entity with id: {id} was not found.");
            }

            return entity;
        }

        public bool DeleteById<T, TId>(TId id, IGenericRepository<T, TId> repository) where T : class
        {
            var entity = GetById(id, repository);
            if (entity == null)
            {
                return false;
            }

            repository.Delete(entity);
            UnitOfWork.Save();
            return true;
        }

        protected void ValidateId<TId>(TId id)
        {
            switch (typeof(TId).Name)
            {
                case nameof(Int64): // long is Int64
                    long? longId = id as long?;
                    if (!longId.HasValue || longId.Value <= 0)
                    {
                        throw new ArgumentOutOfRangeException($"{nameof(id)} must be greater than 0.");
                    }
                    break;

                case nameof(String):
                    string stringId = id as string;
                    if (string.IsNullOrWhiteSpace(stringId))
                    {
                        throw new ArgumentOutOfRangeException($"{nameof(id)} must be a non empty string.");
                    }
                    break;

                default:
                    throw new BusinnessException("Unable to validate Id");
            }


        }

        #endregion
    }
}
