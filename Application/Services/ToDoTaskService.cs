using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Models;
using Domain.Repositories.Interfaces;
using MapsterMapper;

namespace Application.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private IToDoTaskRepository ToDoTaskRepository { get; init; }
        private IMapper Mapper { get; init; }

        public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, IMapper mapper)
        {
            ToDoTaskRepository = toDoTaskRepository;
            Mapper = mapper;
        }

        public async Task<ToDoTaskResponseDTO> CreateToDoTaskAsync(ToDoTaskCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Task title cannot be empty.");
            }

            ToDoTask task = Mapper.Map<ToDoTask>(dto);
            ToDoTask result = await ToDoTaskRepository.CreateToDoTaskAsync(task);
            return Mapper.Map<ToDoTaskResponseDTO>(result);
        }

        public async Task<ToDoTaskResponseDTO> GetToDoTaskByIdAsync(Guid taskId)
        {
            ToDoTask? task = await ToDoTaskRepository.GetToDoTaskByIdAsync(taskId);
            if (task is null)
            {
                throw new Exception($"ToDoTask by id {taskId} does not found.");
            }
            return Mapper.Map<ToDoTaskResponseDTO>(task);
        }

        public async Task<ToDoTaskResponseDTO> GetToDoTaskByIdIncludeStepsAndCategoryAsync(Guid taskId)
        {
            ToDoTask? task = await ToDoTaskRepository.GetToDoTaskByIdIncludeStepsAndCategoryAsync(taskId);
            if (task is null)
            {
                throw new Exception($"ToDoTask by id {taskId} does not found.");
            }
            return Mapper.Map<ToDoTaskResponseDTO>(task);
        }

        public async Task<IEnumerable<ToDoTaskResponseDTO>> GetModelAllToDoTasksAsync()
        {
            var tasks = await ToDoTaskRepository.GetAllToDoTasksAsync();
            return Mapper.Map<IEnumerable<ToDoTaskResponseDTO>>(tasks);
        }

        public async Task<IEnumerable<ToDoTaskResponseDTO>> GetMyDayToDoTasksAsync()
        {
            var tasks = await ToDoTaskRepository.GetMyDayToDoTasksAsync();
            return Mapper.Map<IEnumerable<ToDoTaskResponseDTO>>(tasks);
        }

        public async Task<ToDoTaskResponseDTO> UpdateToDoTaskAsync(ToDoTaskUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Task title cannot be empty.");
            }

            ToDoTask? existingTask = await ToDoTaskRepository.GetToDoTaskByIdAsync(dto.Id);
            if (existingTask is null)
            {
                throw new Exception($"ToDoTask by id {dto.Id} does not found to update.");
            }

            ToDoTask task = Mapper.Map<ToDoTask>(dto);
            ToDoTask result = await ToDoTaskRepository.UpdateToDoTaskAsync(task);
            return Mapper.Map<ToDoTaskResponseDTO>(result);
        }

        public async Task<ToDoTaskResponseDTO> DeleteToDoTaskAsync(Guid taskId)
        {
            ToDoTask? task = await ToDoTaskRepository.GetToDoTaskByIdAsync(taskId);
            if (task is null)
            {
                throw new Exception($"ToDoTask by id {taskId} does not found.");
            }

            ToDoTask result = await ToDoTaskRepository.DeleteToDoTaskAsync(task);
            return Mapper.Map<ToDoTaskResponseDTO>(result);
        }
    }
}
