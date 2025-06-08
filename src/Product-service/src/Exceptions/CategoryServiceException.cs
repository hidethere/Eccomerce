using System.Net;

namespace Product_service.Exceptions
{
    public class CategoryServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public CategoryServiceException(string message, HttpStatusCode statusCode): base(message)
        {
            StatusCode = statusCode;
        }
    }
}
