using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Mapster;

namespace Application.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IToDoTaskRepository _toDoTaskRepository;
        private readonly ICurrentUserService _currentUserService;

        public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, ICurrentUserService currentUserService)
        {
            _toDoTaskRepository = toDoTaskRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ToDoTaskResponseDTO> CreateToDoTaskAsync(ToDoTaskCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Task title cannot be empty.");
            }

            var task = dto.Adapt<ToDoTask>();
            task.AuthorId = _currentUserService.UserId;
            var result = await _toDoTaskRepository.CreateToDoTaskAsync(task);

            return result.Adapt<ToDoTaskResponseDTO>();
        }

        public async Task<ToDoTaskResponseDTO> GetToDoTaskByIdAsync(Guid taskId)
        {
            var task = await _toDoTaskRepository.GetToDoTaskByIdAsync(taskId, _currentUserService.UserId);

            if (task is null)
            {
                throw new Exception($"ToDoTask with id {taskId} was not found.");
            }

            return task.Adapt<ToDoTaskResponseDTO>();
        }

        public async Task<ToDoTaskResponseDTO> GetToDoTaskByIdIncludeStepsAndCategoryAsync(Guid taskId)
        {
            var task = await _toDoTaskRepository.GetToDoTaskByIdIncludeStepsAndCategoryAsync(taskId, _currentUserService.UserId);
            if (task is null)
            {
                throw new Exception($"ToDoTask with id {taskId} was not found.");
            }

            return task.Adapt<ToDoTaskResponseDTO>();
        }

        public async Task<IEnumerable<ToDoTaskResponseDTO>> GetModelAllToDoTasksAsync()
        {
            var tasks = await _toDoTaskRepository.GetAllToDoTasksAsync(_currentUserService.UserId);

            return tasks.Adapt<IEnumerable<ToDoTaskResponseDTO>>();
        }

        public async Task<IEnumerable<ToDoTaskResponseDTO>> GetMyDayToDoTasksAsync()
        {
            var tasks = await _toDoTaskRepository.GetMyDayToDoTasksAsync(_currentUserService.UserId);

            return tasks.Adapt<IEnumerable<ToDoTaskResponseDTO>>();
        }

        public async Task<ToDoTaskResponseDTO> UpdateToDoTaskAsync(Guid taskId, ToDoTaskUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Task title cannot be empty.");
            }

            var existingTask = await _toDoTaskRepository.GetToDoTaskByIdAsync(taskId, _currentUserService.UserId);
            if (existingTask is null)
            {
                throw new Exception($"ToDoTask with id {taskId} was not found to update.");
            }

            dto.Adapt(existingTask);
            var result = await _toDoTaskRepository.UpdateToDoTaskAsync(existingTask);

            return result.Adapt<ToDoTaskResponseDTO>();
        }

        public async Task<ToDoTaskResponseDTO> DeleteToDoTaskAsync(Guid taskId)
        {
            var task = await _toDoTaskRepository.GetToDoTaskByIdAsync(taskId, _currentUserService.UserId);
            if (task is null)
            {
                throw new Exception($"ToDoTask with id {taskId} was not found.");
            }

            var result = await _toDoTaskRepository.DeleteToDoTaskAsync(task);

            return result.Adapt<ToDoTaskResponseDTO>();
        }
    }
}
