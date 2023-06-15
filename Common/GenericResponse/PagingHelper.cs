namespace Common.GenericResponse
{
    public static class PagingHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates a paged set of results.
        /// </summary>
        /// <typeparam name="TReturn">The type of the returned paged results.</typeparam>
        /// <param name="queryable">The source IQueryable.</param>
        /// <param name="page">The page number you want to retrieve.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="orderBy">The field or property to order by.</param>
        /// <param name="ascending">
        /// Indicates whether or not the order should be ascending (true) or descending (false.)
        /// </param>
        /// <returns>Returns a paged set of results.</returns>
        public static async Task<ListModelResponse<TReturn>> CreatePagedResults<TReturn>(
            IQueryable<TReturn> queryable,
            int? page,
            int? pageSize,
            string orderBy,
            bool ascending)
        {
            try
            {
                IQueryable<TReturn> projection;

                if (pageSize == null || pageSize == 0)
                {
                    pageSize = 10;
                }

                if (page != null)
                {
                    var skipAmount = pageSize.Value * (page.Value - 1);

                    projection = queryable
                        .OrderByPropertyOrField(orderBy, ascending)
                        .Skip(skipAmount)
                        .Take(pageSize.Value);
                }
                else
                {
                    projection = queryable
                        .OrderByPropertyOrField(orderBy, ascending);
                }

                var totalNumberOfRecords = queryable.Count();
                var results = projection.ToList();
                var mod = totalNumberOfRecords % pageSize.Value;
                var totalPageCount = (totalNumberOfRecords / pageSize.Value) + (mod == 0 ? 0 : 1);

                return new ListModelResponse<TReturn>
                {
                    Values = results,
                    PageNumber = page ?? 0,
                    PageSize = results.Count,
                    TotalNumberOfPages = page == null ? 1 : totalPageCount,
                    TotalNumberOfRecords = totalNumberOfRecords
                };
            }
            catch (Exception)
            {
                return new ListModelResponse<TReturn>();
            }
        }

        #endregion Public Methods
    }
}
