using Api.Data.Access;
using Api.Data.CosmosDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.CosmosDb.Repositories
{
    /// <summary>
    /// Provides only the bare minimum operations expected in any CosmosDb repository.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"/> of the entities stored the repository.</typeparam>
    /// <typeparam name="TId">The <see cref="Type"/> of the Id of the <see cref="TEntity"/>.</typeparam>
    public interface IGenericRepositoryAsync<TEntity, TId> where TEntity : CosmosModelBase
    {
        #region Get

        /// <summary>
        /// Finds the entity with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<TEntity> GetByIdAsync(string id);

        /// <summary>
        /// Finds the entity with the specified id, using the specified partition key.
        /// Note: Collections that have partition keys must use this overload.
        /// </summary>
        /// <typeparam name="TPartitionKey"></typeparam>
        /// <param name="id"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync<TPartitionKey>(string id, TPartitionKey partitionKey);

        /// <summary>
        /// Gets the page in ascending order
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<PageC<TEntity>> GetPageAsync(string continuationToken, List<Expression<Func<TEntity, bool>>> filters);

        #endregion

        #region Upsert

        /// <summary>
        /// Adds or Updates the specified entity. 
        /// Also called as Upsert.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);

        #endregion

        #region Delete

        Task<bool> DeleteAsync(string resourceId);
        /// <summary>
        /// Note: Collections that have partition keys must use this overload.
        /// </summary>
        /// <typeparam name="TPartitionKey"></typeparam>
        /// <param name="id"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TPartitionKey>(string id, TPartitionKey partitionKey);

        #endregion

        #region Count

        /// <summary>
        /// Returns the total number of entities 
        /// </summary>
        /// <returns></returns>
        Task<long> CountAsync();

        #endregion

    }
}
