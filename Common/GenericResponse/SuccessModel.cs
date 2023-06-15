using System.Runtime.Serialization;

namespace Common.GenericResponse
{
    public class SuccessModel<TModel>
    {
        /// <summary>
        /// Response Code
        /// </summary>
        [DataMember(Name = "responseCode")]
        public string ResponseCode { get; set; }
        /// <summary>
        /// Response Description
        /// </summary>
        [DataMember(Name = "responseDesc")]
        public string ResponseDesc { get; set; }

        /// <summary>
        /// Response Object
        /// </summary>
        [DataMember(Name = "responseObj")]
        public TModel ResponseObj { get; set; }
    }
}
