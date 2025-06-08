using Product_service.Domain;
using System.ComponentModel.DataAnnotations;

namespace Product_service.Dto
{
    public record ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public IFormFile? Image { get; set; }
        public CategoryDto Category { get; set; }
    }
}
