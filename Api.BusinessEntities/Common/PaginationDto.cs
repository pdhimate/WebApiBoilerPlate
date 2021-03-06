﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.Common
{
    /// <summary>
    /// Provides for paginated results.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PagedRes<TEntity>
    {
        public PagedRes(IEnumerable<TEntity> items, long totalCount, int pageNumber, int pageSize)
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
    /// Provides for paginated results.
    /// </summary>
    public class PagedReqDto
    {
        /// <summary>
        /// The page number.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        /// <summary>
        /// The number of entities expected on a page.
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int PageSize { get; set; }
    }
}
