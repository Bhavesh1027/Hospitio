using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static HospitioApi.Shared.GetPagedExtension;

namespace HospitioApi.Shared;
public static partial class GetPagedExtensionV1
{
    public static async Task<SearchResult<T>> GetPagedAsync<T>(this IQueryable<T> query, BaseSearchFilterOptions baseFilter, CancellationToken ct) where T : class
    {

        //Apply sorting
        if (baseFilter.Sort.Any())
        {
            IOrderedQueryable<T>? orderedQuery = null;

            for (var i = 0; i < baseFilter.Sort.Count(); i++)
            {
                orderedQuery = orderedQuery is null
                    ? query.OrderBy(baseFilter.Sort[i])
                    : orderedQuery.ThenBy(baseFilter.Sort[i]);
            }

            query = orderedQuery ?? query;
        }

        var result = new SearchResult<T>
        {
            RowCount = await query.CountAsync(ct)
        };

        if (baseFilter.PageNo > 0 && baseFilter.PageSize > 0 && result.RowCount != 0)
        {
            result.CurrentPage = baseFilter.PageNo;
            result.PageSize = baseFilter.PageSize;

            //Apply pagination
            var pageCount = (double)result.RowCount / baseFilter.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            int skip;
            if (result.PageCount <= baseFilter.PageNo)
            {
                result.CurrentPage = result.PageCount;
                skip = (result.PageCount - 1) * baseFilter.PageSize;
            }
            else
            {
                skip = (baseFilter.PageNo - 1) * baseFilter.PageSize;
            }

            result.Values = await query.Skip(skip).Take(baseFilter.PageSize).ToListAsync(ct);
        }
        else
        {
            result.CurrentPage = 1;
            result.PageSize = result.RowCount;
            result.PageCount = 1;
            result.Values = await query.ToListAsync(ct);
        }

        return result;
    }

    public class SearchResult<T> : SearchResultBase where T : class
    {
        public List<T> Values { get; set; } = new();
    }
}
