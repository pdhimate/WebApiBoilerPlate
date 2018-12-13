using Api.Data.Access.Repositories;
using Api.Data.Access.Repositories.Security;
using Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;

namespace Api.Data.Access
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork, IDisposable
    {
        #region Private members

        private readonly AppDatabaseContext _context;

        #endregion

        public EntityFrameworkUnitOfWork()
        {
            _context = new AppDatabaseContext();
        }

        #region IUnitOfWork implementation

        #region Security

        private IUserRepository _appUserRepository;
        public IUserRepository AppUsers
        {
            get
            {
                if (_appUserRepository == null)
                {
                    _appUserRepository = new UserRepository(_context);
                }
                return _appUserRepository;
            }
        }

        private IAppRoleRepository _appRoleRepository;
        public IAppRoleRepository AppRoles
        {
            get
            {
                if (_appRoleRepository == null)
                {
                    _appRoleRepository = new AppRoleRepository(_context);
                }
                return _appRoleRepository;
            }
        }

        private IAppUserAppRoleMappingRepo _appUserRoleMapRepo;
        public IAppUserAppRoleMappingRepo AppUserRoleMap
        {
            get
            {
                if (_appUserRoleMapRepo == null)
                {
                    _appUserRoleMapRepo = new AppUserAppRoleMappingRepo(_context);
                }
                return _appUserRoleMapRepo;
            }
        }

        private IRefreshTokenRepo _refreshTokensRepo;
        public IRefreshTokenRepo RefreshTokens
        {
            get
            {
                if (_refreshTokensRepo == null)
                {
                    _refreshTokensRepo = new RefreshTokenRepo(_context);
                }
                return _refreshTokensRepo;
            }
        }

        private IExternalUserLoginRepository _externalUserLoginRepository;
        public IExternalUserLoginRepository ExternalUserLogins
        {
            get
            {
                if (_externalUserLoginRepository == null)
                {
                    _externalUserLoginRepository = new ExternalUserLoginRepository(_context);
                }
                return _externalUserLoginRepository;
            }
        }

        #endregion

        #region Public repositories/properties

        private IAddressRepository _addresses;
        public IAddressRepository Addresses
        {
            get
            {
                if (_addresses == null)
                {
                    _addresses = new AddressRepository(_context);
                }
                return _addresses;
            }
        }

        #endregion

        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                // TODO: Handle exceptions and Log errors to db. Probably try to create a new instance of db context. If that fails, log to a file somewhere.
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw;
            }
        }

        #endregion

        #region IDisposable implementation

        private bool disposed = false;

        /// <summary>  
        /// Protected Virtual Dispose method  
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        /// <summary>  
        /// Dispose method  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
