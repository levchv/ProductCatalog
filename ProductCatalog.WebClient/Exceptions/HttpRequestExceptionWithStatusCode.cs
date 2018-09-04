using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalog.WebClient.Exceptions
{
    public class HttpRequestExceptionWithStatusCode: HttpRequestException
    {
        public HttpRequestExceptionWithStatusCode(HttpStatusCode statusCode, object value = null)
        {
            StatusCode = statusCode;
            Value = value;
        }

        public HttpStatusCode StatusCode { get; }
        public object Value { get;  }
    }
}
