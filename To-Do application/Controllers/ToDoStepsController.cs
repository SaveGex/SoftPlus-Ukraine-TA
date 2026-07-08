using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoStepsController : ControllerBase
    {
        private IToDoStepService ToDoStepService { get; init; }

        public ToDoStepsController(IToDoStepService toDoStepService)
        {
            ToDoStepService = toDoStepService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToDoStepResponseDTO), StatusCodes.Status201Created)]
        public async Task<IResult> CreateToDoStep([FromBody] ToDoStepCreateDTO dto)
        {
            ToDoStepResponseDTO result;
            try
            {
                result = await ToDoStepService.CreateToDoStepAsync(dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.Json(result, statusCode: StatusCodes.Status201Created);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToDoStepResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ToDoStepResponseDTO>> GetStepById(Guid id)
        {
            ToDoStepResponseDTO result;
            try
            {
                result = await ToDoStepService.GetToDoStepByIdAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet("task/{taskId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<ToDoStepResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ToDoStepResponseDTO>>> GetStepsByTaskId(Guid taskId)
        {
            IEnumerable<ToDoStepResponseDTO> result;
            try
            {
                result = await ToDoStepService.GetToDoStepsByTaskIdAsync(taskId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IResult> UpdateToDoStep(Guid id, [FromBody] ToDoStepUpdateDTO dto)
        {
            try
            {
                await ToDoStepService.UpdateToDoStepAsync(id, dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IResult> DeleteToDoStep(Guid id)
        {
            try
            {
                await ToDoStepService.DeleteToDoStepAsync(id);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.NoContent();
        }
    }
}
