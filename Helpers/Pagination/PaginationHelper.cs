using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers.Pagination
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }

        public string Message { get; set; } = string.Empty;
    }

    public class PaginationHelper<T>
    {
        public PaginationResult<T> GetPaginatedData(
            IEnumerable<T> source,
            int page,
            int pageSize,
            string? sortField = null,
            string? sortOrder = null,
            Func<T, bool>? filterFunc = null)
        {
            // Filter
            if (filterFunc != null)
            {
                source = source.Where(filterFunc);
            }

            // Sort
            if (!string.IsNullOrEmpty(sortField))
            {
                source = ApplySorting(source, sortField, sortOrder);
            }

            // Load pageSize + 1 to detect if there's a next page
            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize + 1)
                .ToList();

            return new PaginationResult<T>
            {
                Items = items.Take(pageSize).ToList(),
                Page = page,
                PageSize = pageSize,
                HasNextPage = items.Count > pageSize
            };
        }

        private IEnumerable<T> ApplySorting(IEnumerable<T> data, string sortField, string sortOrder)
        {
            var prop = typeof(T).GetProperty(sortField);
            if (prop == null) return data;

            return sortOrder.ToLower() == "asc"
                ? data.OrderBy(x => prop.GetValue(x))
                : data.OrderByDescending(x => prop.GetValue(x));
        }
    }
}
