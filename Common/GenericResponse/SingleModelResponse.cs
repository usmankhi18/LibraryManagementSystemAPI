using Newtonsoft.Json;

namespace Common.GenericResponse
{
    public class SingleModelResponse<TModel> : IDisposable
    {
        #region Public Properties

        [JsonIgnore]
        public ErrorResponseModel Error { get; set; }

        [JsonIgnore]
        public bool IsError { get; set; }
        [JsonIgnore]
        public SuccessModel<TModel> Model { get; set; }
        [JsonIgnore]
        public ListModelResponse<TModel> ListModel { get; set; }

        public void Dispose()
        {

        }

        #endregion Public Properties
    }
}
