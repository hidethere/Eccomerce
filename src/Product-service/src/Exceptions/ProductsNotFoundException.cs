using System.Net;

namespace Product_service.Exceptions
{
    public class ProductsNotFoundException : ProductServiceException
    {
        public ProductsNotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode) { }

    }
}
