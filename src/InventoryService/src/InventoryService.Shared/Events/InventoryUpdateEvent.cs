using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Shared.Events
{
    internal class InventoryUpdateEvent
    {
        public Guid ProductId { get; set; }
        public int NewQuantity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
