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
    internal class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, Inventory>
    {
        private readonly IInventoryRepository _inventoryRepository;
        public CreateInventoryCommandHandler(IInventoryRepository inventoryRepository) 
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventory = new Inventory
            {
                ProductId = request.ProductId,
                QuantityAvailable = request.QuantityAvailable,
                LastUpdated = DateTime.UtcNow,
            };

            await _inventoryRepository.AddAsync(inventory);

            return inventory;
        }
    }
}
