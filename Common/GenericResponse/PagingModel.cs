using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Common.GenericResponse
{
    public partial class PagingModel
    {
        #region Public Properties

        [JsonProperty(PropertyName = "ascending")]
        public bool Ascending { get; set; } = true;

        [JsonProperty(PropertyName = "orderBy")]
        [RegularExpression(@"[a-zA-Z]+$", ErrorMessage = "PG001" + ":Order By field has invalid characters.")]
        public string? OrderBy { get; set; }

        [Range(1, 100)]
        [JsonProperty(PropertyName = "page")]
        public int? Page { get; set; }

        [Range(1, 1000)]
        [JsonProperty(PropertyName = "pageSize")]
        public int? PageSize { get; set; }

        #endregion Public Properties
    }
}
