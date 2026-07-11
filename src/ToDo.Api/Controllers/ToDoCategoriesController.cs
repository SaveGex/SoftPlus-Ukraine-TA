using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using To_Do_application.Filters.FilterAttributes;

namespace To_Do_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoCategoriesController : ControllerBase
    {
        private IToDoCategoryService ToDoCategoryService { get; init; }

        public ToDoCategoriesController(IToDoCategoryService toDoCategoryService)
        {
            ToDoCategoryService = toDoCategoryService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToDoCategoryResponseDTO), StatusCodes.Status201Created)]
        public async Task<IResult> CreateToDoCategory([FromBody] ToDoCategoryCreateDTO dto)
        {
            ToDoCategoryResponseDTO result;
            try
            {
                result = await ToDoCategoryService.CreateToDoCategoryAsync(dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.Json(result, statusCode: StatusCodes.Status201Created);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ToDoCategoryResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ToDoCategoryResponseDTO>>> GetAllCategories()
        {
            IEnumerable<ToDoCategoryResponseDTO> result;
            try
            {
                result = await ToDoCategoryService.GetAllToDoCategoriesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToDoCategoryResponseDTO), StatusCodes.Status200OK)]
        [Ownership("id", nameof(Domain.Models.ToDoTask))]
        public async Task<ActionResult<ToDoCategoryResponseDTO>> GetCategoryById(Guid id)
        {
            ToDoCategoryResponseDTO result;
            try
            {
                result = await ToDoCategoryService.GetToDoCategoryByIdAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}/tasks")]
        [ProducesResponseType(typeof(ToDoCategoryResponseDTO), StatusCodes.Status200OK)]
        [Ownership("id", nameof(Domain.Models.ToDoTask))]
        public async Task<ActionResult<ToDoCategoryResponseDTO>> GetCategoryWithTasks(Guid id)
        {
            ToDoCategoryResponseDTO result;
            try
            {
                result = await ToDoCategoryService.GetToDoCategoryByIdIncludeTasksAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Ownership("id", nameof(Domain.Models.ToDoTask))]
        public async Task<IResult> UpdateToDoCategory(Guid id, [FromBody] ToDoCategoryUpdateDTO dto)
        {
            try
            {
                await ToDoCategoryService.UpdateToDoCategoryAsync(id, dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Ownership("id", nameof(Domain.Models.ToDoTask))]
        public async Task<IResult> DeleteToDoCategory(Guid id)
        {
            try
            {
                await ToDoCategoryService.DeleteToDoCategoryAsync(id);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.NoContent();
        }
    }
}
