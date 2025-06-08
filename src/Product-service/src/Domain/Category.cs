using System.ComponentModel.DataAnnotations;

namespace Product_service.Domain
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
