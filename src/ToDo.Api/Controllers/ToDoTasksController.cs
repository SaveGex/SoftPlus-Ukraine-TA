using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using To_Do_application.Filters.FilterAttributes;

namespace To_Do_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private IToDoTaskService ToDoTaskService { get; init; }
        private const string TASKS_KEY = nameof(Domain.Models.ToDoTask);

        public ToDoTasksController(IToDoTaskService toDoTaskService)
        {
            ToDoTaskService = toDoTaskService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToDoTaskResponseDTO), StatusCodes.Status201Created)]
        public async Task<IResult> CreateToDoTask([FromBody] ToDoTaskCreateDTO dto)
        {
            ToDoTaskResponseDTO result;
            try
            {
                result = await ToDoTaskService.CreateToDoTaskAsync(dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

            return Results.Json(result, statusCode: StatusCodes.Status201Created);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ToDoTaskResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ToDoTaskResponseDTO>>> GetAllTasks()
        {
            IEnumerable<ToDoTaskResponseDTO> result;
            try
            {
                result = await ToDoTaskService.GetModelAllToDoTasksAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet("my-day")]
        [ProducesResponseType(typeof(IEnumerable<ToDoTaskResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ToDoTaskResponseDTO>>> GetMyDayTasks()
        {
            IEnumerable<ToDoTaskResponseDTO> result;
            try
            {
                result = await ToDoTaskService.GetMyDayToDoTasksAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToDoTaskResponseDTO), StatusCodes.Status200OK)]
        [Ownership("id", TASKS_KEY)]
        public async Task<ActionResult<ToDoTaskResponseDTO>> GetTaskById(Guid id)
        {
            ToDoTaskResponseDTO result;
            try
            {
                result = await ToDoTaskService.GetToDoTaskByIdAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}/details")]
        [ProducesResponseType(typeof(ToDoTaskResponseDTO), StatusCodes.Status200OK)]
        [Ownership("id", TASKS_KEY)]
        public async Task<ActionResult<ToDoTaskResponseDTO>> GetTaskWithDetails(Guid id)
        {
            ToDoTaskResponseDTO result;
            try
            {
                result = await ToDoTaskService.GetToDoTaskByIdIncludeStepsAndCategoryAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Ownership("id", TASKS_KEY)]
        public async Task<IResult> UpdateToDoTask(Guid id, [FromBody] ToDoTaskUpdateDTO dto)
        {
            try
            {
                await ToDoTaskService.UpdateToDoTaskAsync(id, dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Ownership("id", TASKS_KEY)]
        public async Task<IResult> DeleteToDoTask(Guid id)
        {
            try
            {
                await ToDoTaskService.DeleteToDoTaskAsync(id);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.NoContent();
        }
    }
}
