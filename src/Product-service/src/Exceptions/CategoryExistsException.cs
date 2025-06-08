using System.Net;

namespace Product_service.Exceptions
{
    public class CategoryExistsException : CategoryServiceException
    {
        public CategoryExistsException(string message, HttpStatusCode statusCode): base(message, statusCode) { }
    }
}
