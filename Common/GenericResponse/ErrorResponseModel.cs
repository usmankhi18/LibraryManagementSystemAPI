using Newtonsoft.Json;
using System.Net;

namespace Common.GenericResponse
{
    [JsonObject(Title = "error")]
    public class ErrorResponseModel
    {
        [JsonIgnore]
        public IList<KeyValuePair<string, string>> _errorParameters;
        [JsonIgnore]
        public List<ErrorResponseModel> _errorParameters2;
        [JsonProperty(PropertyName = "code")]
        public string ErrorCode { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "errorCategory")]
        public string ErrorCategory;
        [JsonProperty(PropertyName = "target")]
        public string Target;
        [JsonIgnore]
        public string ErrorDateTime { get; set; }
        [JsonIgnore]
        public IList<KeyValuePair<string, string>> ErrorParameters
        {
            get
            {
                return _errorParameters;
            }
            set
            {
                this._errorParameters = value;
                Details = new List<ErrorResponseModel>();
                foreach (var each in _errorParameters.ToList())
                {
                    Details.Add(new ErrorResponseModel
                    {
                        ErrorCode = each.Key,
                        Description = each.Value
                    });
                }
            }
        }
        [JsonProperty(PropertyName = "details")]
        public List<ErrorResponseModel> Details
        {
            get
            {
                _errorParameters2 = new List<ErrorResponseModel>();
                if (ErrorParameters != null && ErrorParameters.Count > 0)
                {
                    foreach (var each in ErrorParameters.ToList())
                    {
                        _errorParameters2.Add(new ErrorResponseModel
                        {
                            ErrorCode = each.Key,
                            Description = each.Value
                        });
                    }
                }
                if (_errorParameters2.Count == 0)
                {
                    return null;
                }
                else
                {
                    return _errorParameters2;
                }
            }
            set { this._errorParameters2 = value; }
        }
        [JsonIgnore]
        public IList<Link> Links { get; set; }

        public ErrorResponseModel()
        {
            Links = new List<Link>();
        }
    }

    public static class ResponseCategory
    {
        public const string BusinessRule = "BusinessRule";
        public const string Validation = "Validation";
        public const string Authorization = "Authorization";
        public const string Identification = "Identification";
        public const string InternalServer = "InternalServer";
        public const string ServiceUnavailable = "ServiceUnavailable";


        public static HttpStatusCode GetHttpResp(string RespCatagory)
        {
            switch (RespCatagory)
            {
                case BusinessRule:
                    return HttpStatusCode.Forbidden;
                case Validation:
                    return HttpStatusCode.BadRequest;
                case Authorization:
                    return HttpStatusCode.Unauthorized;
                case Identification:
                    return HttpStatusCode.NotFound;
                case InternalServer:
                    return HttpStatusCode.InternalServerError;
                case ServiceUnavailable:
                    return HttpStatusCode.ServiceUnavailable;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }

    public class Link
    {

        public Link(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }


    }
}
