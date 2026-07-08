using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Models;
using Domain.Repositories.Interfaces;
using MapsterMapper;

namespace Application.Services
{
    public class ToDoStepService : IToDoStepService
    {
        private IToDoStepRepository ToDoStepRepository { get; init; }
        private IMapper Mapper { get; init; }

        public ToDoStepService(IToDoStepRepository toDoStepRepository, IMapper mapper)
        {
            ToDoStepRepository = toDoStepRepository;
            Mapper = mapper;
        }

        public async Task<ToDoStepResponseDTO> CreateToDoStepAsync(ToDoStepCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Step name cannot be empty.");
            }

            ToDoStep step = Mapper.Map<ToDoStep>(dto);
            ToDoStep result = await ToDoStepRepository.CreateToDoStepAsync(step);
            return Mapper.Map<ToDoStepResponseDTO>(result);
        }

        public async Task<ToDoStepResponseDTO> GetToDoStepByIdAsync(Guid stepId)
        {
            ToDoStep? step = await ToDoStepRepository.GetToDoStepByIdAsync(stepId);
            if (step is null)
            {
                throw new Exception($"ToDoStep by id {stepId} does not found.");
            }
            return Mapper.Map<ToDoStepResponseDTO>(step);
        }

        public async Task<IEnumerable<ToDoStepResponseDTO>> GetToDoStepsByTaskIdAsync(Guid taskId)
        {
            var steps = await ToDoStepRepository.GetToDoStepsByTaskIdAsync(taskId);
            return Mapper.Map<IEnumerable<ToDoStepResponseDTO>>(steps);
        }

        public async Task<ToDoStepResponseDTO> UpdateToDoStepAsync(ToDoStepUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Step name cannot be empty.");
            }

            ToDoStep? existingStep = await ToDoStepRepository.GetToDoStepByIdAsync(dto.Id);
            if (existingStep is null)
            {
                throw new Exception($"ToDoStep by id {dto.Id} does not found to update.");
            }

            ToDoStep step = Mapper.Map<ToDoStep>(dto);
            ToDoStep result = await ToDoStepRepository.UpdateToDoStepAsync(step);
            return Mapper.Map<ToDoStepResponseDTO>(result);
        }

        public async Task<ToDoStepResponseDTO> DeleteToDoStepAsync(Guid stepId)
        {
            ToDoStep? step = await ToDoStepRepository.GetToDoStepByIdAsync(stepId);
            if (step is null)
            {
                throw new Exception($"ToDoStep by id {stepId} does not found.");
            }

            ToDoStep result = await ToDoStepRepository.DeleteToDoStepAsync(step);
            return Mapper.Map<ToDoStepResponseDTO>(result);
        }
    }
}
