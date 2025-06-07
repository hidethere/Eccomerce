using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Infrastructure.Entities;

namespace InventoryService.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        public Task<Inventory> GetByIdAsync(Guid id);
        public Task<Inventory> GetByProductIdAsync(Guid productId);
        public Task<Inventory> AddAsync(Inventory inventory);
        Task UpdateAsync(Inventory inventory);

    }
}
