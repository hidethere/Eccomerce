using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Domain.Events
{
    public class ProductSoldEvent
    {
        public Guid ProductId { get; set; }
        public int QuantitySold { get; set; }

    }
}
