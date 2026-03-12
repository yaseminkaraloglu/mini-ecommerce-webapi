using Microsoft.AspNetCore.Mvc;
using MyE_CommerceWebAPI.Dtos.Category;
using MyE_CommerceWebAPI.Services.Interfaces;

namespace MyE_CommerceWebAPI.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class CategoriesController : ControllerBase
        {
            private readonly ICategoryService _service;


            public CategoriesController(ICategoryService service)
            {
                _service = service;
            }


            [HttpGet]
            public async Task<ActionResult<List<CategoryDto>>> GetAll([FromQuery] bool onlyActive = true)
            {
                var data = await _service.GetAllAsync(onlyActive);
                return Ok(data);
            }


            [HttpGet("{id:int}")]
            public async Task<ActionResult<CategoryDto>> GetById(int id)
            {
                var category = await _service.GetByIdAsync(id);
                if (category == null) return NotFound();


                return Ok(category);
            }


            [HttpPost]
            public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
            {
                if (dto == null) return BadRequest();
                if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name boş olamaz.");


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


            [HttpPut("{id:int}")]
            public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
            {
                if (dto == null) return BadRequest();
                if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name boş olamaz.");


                try
                {
                    var updated = await _service.UpdateAsync(id, dto);
                    if (updated == null) return NotFound();


                    return Ok(updated);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }


            [HttpDelete("{id:int}")]
            public async Task<IActionResult> SoftDelete(int id)
            {
                var ok = await _service.SoftDeleteAsync(id);
                if (!ok) return NotFound();


                return NoContent();
            }
        }
    
}

