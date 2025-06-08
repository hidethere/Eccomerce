using System.Net;

namespace Product_service.Exceptions
{
    public class ProductExistsException : ProductServiceException
    {
        public ProductExistsException(string message, HttpStatusCode statusCode): base(message, statusCode) { }
    }
}
