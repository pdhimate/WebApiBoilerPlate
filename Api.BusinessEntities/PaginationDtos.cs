using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities
{
    /// <summary>
    /// Provides for paginated NoSql results.
    /// C for cosmos db.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PagedResC<TEntity>
    {
        public PagedResC(IEnumerable<TEntity> items, string continuationToken)
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
        /// The total number of objects in the database.
        /// </summary>
        public string ContinuationToken { get { return _continuationToken; } }

        #endregion
    }

    /// <summary>
    /// Provides for paginated NoSql results.
    /// </summary>
    public class PagedReqC
    {
        /// <summary>
        /// The token that allows you to fetch the next page.
        /// If this is the first page then set this to null.
        /// </summary>
        public string ContinuationToken { get; set; }
    }
}
