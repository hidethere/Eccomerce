using Asp.Versioning;
using InventoryService.Application.Commands;
using InventoryService.Application.Queries;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InventoryController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpPost]
        [ProducesResponseType<Inventory>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryCommand request)
        {
            var result = await _mediator.Send(request);

            return result == null ? BadRequest() : Ok(result);
        }


        [MapToApiVersion(1)]
        [HttpGet("{productId}")]
        [ProducesResponseType<Inventory>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInventoryByProductId(Guid productId)
        {
            var query = new GetInventoryByProductIdQuery(productId);
            var result = await _mediator.Send(query);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
