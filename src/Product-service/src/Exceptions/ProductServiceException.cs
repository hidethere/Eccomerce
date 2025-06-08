using System.Net;

namespace Product_service.Exceptions
{
    public class ProductServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ProductServiceException(string message, HttpStatusCode statusCode): base(message)
        {
            StatusCode = statusCode;
        }
    }
}
