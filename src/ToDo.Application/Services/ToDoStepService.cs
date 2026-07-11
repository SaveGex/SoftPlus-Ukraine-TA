using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Mapster;

namespace Application.Services
{
    public class ToDoStepService : IToDoStepService
    {
        private readonly IToDoStepRepository _toDoStepRepository;

        public ToDoStepService(IToDoStepRepository toDoStepRepository)
        {
            _toDoStepRepository = toDoStepRepository;
        }

        public async Task<ToDoStepResponseDTO> CreateToDoStepAsync(ToDoStepCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Step name cannot be empty.");
            }

            var step = dto.Adapt<ToDoStep>();
            var result = await _toDoStepRepository.CreateToDoStepAsync(step);

            return result.Adapt<ToDoStepResponseDTO>();
        }

        public async Task<ToDoStepResponseDTO> GetToDoStepByIdAsync(Guid stepId)
        {
            var step = await _toDoStepRepository.GetToDoStepByIdAsync(stepId);
            if (step is null)
            {
                throw new Exception($"ToDoStep with id {stepId} was not found.");
            }

            return step.Adapt<ToDoStepResponseDTO>();
        }

        public async Task<IEnumerable<ToDoStepResponseDTO>> GetToDoStepsByTaskIdAsync(Guid taskId)
        {
            var steps = await _toDoStepRepository.GetToDoStepsByTaskIdAsync(taskId);

            return steps.Adapt<IEnumerable<ToDoStepResponseDTO>>();
        }

        public async Task<ToDoStepResponseDTO> UpdateToDoStepAsync(Guid stepId, ToDoStepUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Step name cannot be empty.");
            }

            var existingStep = await _toDoStepRepository.GetToDoStepByIdAsync(stepId);
            if (existingStep is null)
            {
                throw new Exception($"ToDoStep with id {stepId} was not found to update.");
            }

            dto.Adapt(existingStep);
            var result = await _toDoStepRepository.UpdateToDoStepAsync(existingStep);

            return result.Adapt<ToDoStepResponseDTO>();
        }

        public async Task<ToDoStepResponseDTO> DeleteToDoStepAsync(Guid stepId)
        {
            var step = await _toDoStepRepository.GetToDoStepByIdAsync(stepId);
            if (step is null)
            {
                throw new Exception($"ToDoStep with id {stepId} was not found.");
            }

            var result = await _toDoStepRepository.DeleteToDoStepAsync(step);

            return result.Adapt<ToDoStepResponseDTO>();
        }
    }
}
