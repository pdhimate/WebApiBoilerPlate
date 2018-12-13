using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Data.Access.Repositories
{
    /// <summary>
    /// Provides for implementation of <see cref="IGenericRepository{TEntity, TId}"/> using EntityFramework.
    /// Every repository must inherit from this class.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"/> of the entities stored the repository.</typeparam>
    /// <typeparam name="TId">The <see cref="Type"/> of the Id of the <see cref="TEntity"/>.</typeparam>
    public abstract class EntityFrameworkRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
    {
        #region Protected members

        protected readonly AppDatabaseContext Context;
        protected readonly DbSet<TEntity> DbSet;

        #endregion

        public EntityFrameworkRepository(AppDatabaseContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        #region Public methods

        public virtual TEntity GetById(TId id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// /// Note: Some custom logic to fetch the results has been added here, instead of, in the BusinessLayer.
        /// The idea is to make the repository more useful since it is already caching entities into the memory.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="idSelector">The lamba expression that selects the Id/Key of the <see cref="TEntity"/></param>
        /// <returns></returns>
        public Page<TEntity> GetByPage(int pageNumber, int pageSize, Expression<Func<TEntity, TId>> idSelector)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Invalid pageNumber: {pageNumber} and pageSize: {pageSize}. They must be greater than 0.");
            }

            if (idSelector == null)
            {
                throw new ArgumentNullException(nameof(idSelector));
            }

            var totalCount = DbSet.LongCount();
            double pages = totalCount / pageSize;
            var pageCount = Math.Ceiling(pages);
            var skip = (pageNumber - 1) * pageSize;
            var entities = DbSet.OrderBy(idSelector).Skip(skip).Take(pageSize).ToList();

            var pagedResult = new Page<TEntity>(entities, totalCount, pageNumber, pageSize);
            return pagedResult;
        }

        /// <summary>  
        /// generic Insert method for the entities  
        /// </summary>  
        /// <param name="entity"></param>  
        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>  
        /// Generic Delete method for the entities  
        /// </summary>  
        /// <param name="entityToDelete"></param>  
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Delete(TId entityId)
        {
            var entity = GetById(entityId);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual int Count()
        {
            return DbSet.Count();
        }

       

        #endregion
    }

    /// <summary>
    /// Provides for implementation of <see cref="IGenericRepository{TEntity, TId0, TId1}"/> using EntityFramework.
    /// Every repository must inherit from this class.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"/> of the entities stored the repository.</typeparam>
    /// <typeparam name="TId0">The <see cref="Type"/> of the composite PK Id of order 0 of the <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TId1">The <see cref="Type"/> of the composite PK Id of order 1 of the <see cref="TEntity"/>.</typeparam>
    public abstract class EntityFrameworkRepository<TEntity, TId0, TId1> : IGenericRepository<TEntity, TId0, TId1> where TEntity : class
    {
        #region Protected members

        protected readonly AppDatabaseContext Context;
        protected readonly DbSet<TEntity> DbSet;

        #endregion

        public EntityFrameworkRepository(AppDatabaseContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        #region IGenericRepository implementation

        public TEntity GetById(TId0 idOrder0, TId1 idOrder1)
        {
            return DbSet.Find(idOrder0, idOrder1);
        }

        #endregion
    }

    /// <summary>
    /// Provides for implementation of <see cref="IGenericRepository{TEntity, TId0, TId1, TId2}"/> using EntityFramework.
    /// Every repository must inherit from this class.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"/> of the entities stored the repository.</typeparam>
    /// <typeparam name="TId0">The <see cref="Type"/> of the composite PK Id of order 0 of the <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TId1">The <see cref="Type"/> of the composite PK Id of order 1 of the <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TId2">The <see cref="Type"/> of the composite PK Id of order 2 of the <see cref="TEntity"/>.</typeparam>
    public abstract class EntityFrameworkRepository<TEntity, TId0, TId1, TId2> : IGenericRepository<TEntity, TId0, TId1, TId2> where TEntity : class
    {
        #region Protected members

        protected readonly AppDatabaseContext Context;
        protected readonly DbSet<TEntity> DbSet;

        #endregion

        public EntityFrameworkRepository(AppDatabaseContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        #region IGenericRepository implementation

        public TEntity GetById(TId0 idOrder0, TId1 idOrder1, TId2 idOrder2)
        {
            return DbSet.Find(idOrder0, idOrder1, idOrder2);
        }

        #endregion
    }
}
