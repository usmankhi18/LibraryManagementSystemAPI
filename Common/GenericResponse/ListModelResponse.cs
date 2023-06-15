using Newtonsoft.Json;

namespace Common.GenericResponse
{
    public class ListModelResponse<TModel> : IDisposable
    {
        #region Public Properties

        [JsonProperty(PropertyName = "values")]
        public IEnumerable<TModel> Values { get; set; }

        [JsonProperty(PropertyName = "pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "totalNumberOfPages")]
        public int TotalNumberOfPages { get; set; }

        [JsonProperty(PropertyName = "totalNumberOfRecords")]
        public int TotalNumberOfRecords { get; set; }

        public void Dispose()
        {

        }

        #endregion Public Properties
    }
}
