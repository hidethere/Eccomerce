using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Queries;
using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Entities;
using MediatR;

namespace InventoryService.Application.Handlers
{
    public class GetInventoryByProductQueryHandler : IRequestHandler<GetInventoryByProductIdQuery, Inventory>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public GetInventoryByProductQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> Handle(GetInventoryByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await _inventoryRepository.GetByProductIdAsync(request.ProductId);
        }
    }
}
