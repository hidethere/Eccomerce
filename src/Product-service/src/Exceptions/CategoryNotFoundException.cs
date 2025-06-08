using System.Net;

namespace Product_service.Exceptions
{
    public class CategoryNotFoundException(string message, HttpStatusCode statusCode) : CategoryServiceException(message, statusCode)
    {
    }
}
