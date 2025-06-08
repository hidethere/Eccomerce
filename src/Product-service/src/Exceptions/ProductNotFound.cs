using System.Net;

namespace Product_service.Exceptions
{
    public class ProductNotFound : ProductServiceException
    {
        public ProductNotFound(string message, HttpStatusCode statusCode) : base(message, statusCode) { }

    }
}
