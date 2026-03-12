using Microsoft.AspNetCore.Mvc;
using MyE_CommerceWebAPI.Dtos.Order;
using MyE_CommerceWebAPI.Services.Interfaces;

namespace MyE_CommerceWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto)
        {
            if (dto == null) return BadRequest();
            if (dto.Items == null || dto.Items.Count == 0) return BadRequest("Items boş olamaz.");

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null) return NotFound();

            return Ok(order);
        }
    }
}
