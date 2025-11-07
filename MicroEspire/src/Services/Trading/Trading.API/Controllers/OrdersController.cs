using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trading.Application.Commands;
using Trading.Application.Queries;

namespace Trading.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController: ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetOrder), new { id = result.Value }, result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id, [FromQuery] Guid userId)
        {
            var order = await _mediator.Send(new GetOrderQuery(id, userId));

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderRequest request)
        {
            var result = await _mediator.Send(new CancelOrderCommand(id, request.UserId));

            if (result.IsFailure)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
    public record CancelOrderRequest(Guid UserId);

}
