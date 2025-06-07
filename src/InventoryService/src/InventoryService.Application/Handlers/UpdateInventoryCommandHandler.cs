using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Commands;
using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Entities;
using MediatR;

namespace InventoryService.Application.Handlers
{
    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, Inventory>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public UpdateInventoryCommandHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _inventoryRepository.GetByProductIdAsync(request.ProductId);  

            if(inventory == null)
            {
                throw new ArgumentException("Inventory not found!");
            }

            inventory.QuantityAvailable -= request.QuantitySold;
            await _inventoryRepository.UpdateAsync(inventory);
            return inventory;
        }
    }
}
