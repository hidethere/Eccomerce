using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Product_service.Domain
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); //Cosmo only supports string id's
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength (200, MinimumLength = 4)]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        [Required]
        public Category Category { get; set; }
    }
}
