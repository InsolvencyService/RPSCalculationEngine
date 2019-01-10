using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Insolvency.CalculationsEngine.Redundancy.Common.DTO
{
    public class ResponseDto
    {
        [DataMember] public HttpStatusCode StatusCode { get; set; }

        [DataMember] public bool IsSuccess { get; set; }

        [DataMember] public string Message { get; set; }

        [DataMember] public IList<string> ErrorList { get; set; }
    }

    [DataContract]
    public class ResponseDto<T> : ResponseDto
    {
        [DataMember] public T ResponseData { get; set; }
    }
}