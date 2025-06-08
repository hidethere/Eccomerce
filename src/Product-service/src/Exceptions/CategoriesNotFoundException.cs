using System.Net;

namespace Product_service.Exceptions
{
    public class CategoriesNotFoundException(string message, HttpStatusCode statusCode) : CategoryServiceException(message, statusCode)
    {
    }
}
