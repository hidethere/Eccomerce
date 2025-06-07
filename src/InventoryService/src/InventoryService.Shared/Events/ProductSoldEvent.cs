using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Shared.Events
{
    internal class ProductSoldEvent
    {
        public Guid ProductId { get; set; }
        public int QuantitySold { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
