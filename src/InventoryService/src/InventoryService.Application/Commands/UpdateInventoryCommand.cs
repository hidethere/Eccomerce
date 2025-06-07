using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Infrastructure.Entities;
using MediatR;

namespace InventoryService.Application.Commands
{
    public record UpdateInventoryCommand(Guid ProductId, int QuantitySold) : IRequest<Inventory>;
}
