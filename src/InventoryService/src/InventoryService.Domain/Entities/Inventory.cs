using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Infrastructure.Entities
{
    public class Inventory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public int QuantityAvailable { get; set; }
        public Guid ProductId { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
    }
}
