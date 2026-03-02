using System.Linq.Dynamic.Core;
namespace HospitioApi.Shared;
public static partial class GetPagedExtension
{
    public static SearchResult<T> GetPaged<T>(this IQueryable<T> query, BaseSearchFilterOptions baseFilter) where T : class
    {
        //Apply sorting
        if (baseFilter.Sort.Any())
        {
            IOrderedQueryable<T>? orderedQuery = null;

            for (var i = 0; i < baseFilter.Sort.Count; i++)
            {
                orderedQuery = (orderedQuery is null)
                    ? query.OrderBy(baseFilter.Sort[i])
                    : orderedQuery.ThenBy(baseFilter.Sort[i]);
            }

            query = orderedQuery ?? query;
        }

        var result = new SearchResult<T>
        {
            RowCount = query.Count()
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

            result.Values = query.Skip(skip).Take(baseFilter.PageSize);
        }
        else
        {
            result.CurrentPage = 1;
            result.PageSize = result.RowCount;
            result.PageCount = 1;
            result.Values = query;
        }

        return result;
    }

    public class SearchResult<T> : SearchResultBase where T : class
    {
        public IEnumerable<T> Values { get; set; } = new List<T>();
    }
}

