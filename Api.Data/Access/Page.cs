using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access
{
    /// <summary>
    /// Provides for paginated results.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Page<TEntity>
    {
        public Page(IEnumerable<TEntity> items, long totalCount, int pageNumber, int pageSize)
        {
            _items = items;
            _totalCount = totalCount;
            _pageNumber = pageNumber;
            _pageSize = pageSize;
        }

        #region Properties

        private readonly IEnumerable<TEntity> _items;
        /// <summary>
        /// The collection of objects belonging to this page.
        /// </summary>
        public IEnumerable<TEntity> Items { get { return _items; } }

        private readonly long _totalCount;
        /// <summary>
        /// The total number of objects in the database.
        /// </summary>
        public long TotalCount { get { return _totalCount; } }

        private readonly int _pageNumber;
        /// <summary>
        /// The current page number.
        /// </summary>
        public int PageNumber { get { return _pageNumber; } }

        private readonly int _pageSize;
        /// <summary>
        /// The current page size.
        /// </summary>
        public int PageSize { get { return _pageSize; } }

        #endregion
    }

    /// <summary>
    /// Provides for paginated results for Cosmos db.
    /// C for cosmosDb.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PageC<TEntity>
    {
        public PageC(IEnumerable<TEntity> items, string continuationToken)
        {
            _items = items;
            _continuationToken = continuationToken;
        }

        #region Properties

        private readonly IEnumerable<TEntity> _items;
        /// <summary>
        /// The collection of objects belonging to this page.
        /// </summary>
        public IEnumerable<TEntity> Items { get { return _items; } }

        private readonly string _continuationToken;
        /// <summary>
        /// The token that can be used to fetch the next page.
        /// </summary>
        public string ContinuationToken { get { return _continuationToken; } }

        #endregion
    }
}
