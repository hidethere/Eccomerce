using Product_service.Domain;
using System.ComponentModel.DataAnnotations;

namespace Product_service.Dto
{
    public record CategoryDto
    {
        public string Name { get; set; }
    }
}
