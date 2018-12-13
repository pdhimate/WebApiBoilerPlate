using System;
using System.Linq.Expressions;

namespace Api.Data.Access.Repositories
{
    /// <summary>
    /// Provides only the bare minimum operations expected in any repository.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"/> of the entities stored the repository.</typeparam>
    /// <typeparam name="TId">The <see cref="Type"/> of the Id of the <see cref="TEntity"/>.</typeparam>
    public interface IGenericRepository<TEntity, TId> where TEntity : class
    {
        /// <summary>
        /// Finds the entity with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(TId id);

        /// <summary>
        /// Returns a <see cref="Page{TEntity}"/> from the entire entity set.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="idSelector">The lamba expression that selects the Id/Key of the <see cref="TEntity"/></param>
        /// <returns></returns>
        Page<TEntity> GetByPage(int pageNumber, int pageSize, Expression<Func<TEntity, TId>> idSelector);

        /// <summary>
        /// Inserts the specified new entity.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>  
        /// Updates the specified entity. 
        /// </summary>  
        /// <param name="entityToUpdate"></param>  
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entityToDelete"></param>
        void Delete(TEntity entityToDelete);

        void Delete(TId entityId);

        /// <summary>
        /// Returns the total number of entities in the
        /// </summary>
        /// <returns></returns>
        int Count();
    }

    /// <summary>
    /// Repository with composite PK consisting of 2 columns and with bare minimum operations expected in any repository.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId0"></typeparam>
    /// <typeparam name="TId1"></typeparam>
    public interface IGenericRepository<TEntity, TId0, TId1> where TEntity : class
    {
        /// <summary>
        /// Finds the entity with the specified entity.
        /// </summary>
        /// <param name="idOrder0"></param>
        /// <param name="idOrder1"></param>
        /// <returns></returns>
        TEntity GetById(TId0 idOrder0, TId1 idOrder1);
    }

    /// <summary>
    /// Repository with composite PK consisting of 3 columns and with bare minimum operations expected in any repository.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId0"></typeparam>
    /// <typeparam name="TId1"></typeparam>
    /// <typeparam name="TId2"></typeparam>
    public interface IGenericRepository<TEntity, TId0, TId1, TId2> where TEntity : class
    {
        /// <summary>
        /// Finds the entity with the specified entity.
        /// </summary>
        /// <param name="idOrder0"></param>
        /// <returns></returns>
        TEntity GetById(TId0 idOrder0, TId1 idOrder1, TId2 idOrder2);
    }
}