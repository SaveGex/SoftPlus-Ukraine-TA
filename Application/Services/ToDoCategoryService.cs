using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Models;
using Domain.Repositories.Interfaces;
using MapsterMapper;

namespace Application.Services
{
    public class ToDoCategoryService : IToDoCategoryService
    {
        private IToDoCategoryRepository ToDoCategoryRepository { get; init; }
        private IMapper Mapper { get; init; }

        public ToDoCategoryService(IToDoCategoryRepository toDoCategoryRepository, IMapper mapper)
        {
            ToDoCategoryRepository = toDoCategoryRepository;
            Mapper = mapper;
        }

        public async Task<ToDoCategoryResponseDTO> CreateToDoCategoryAsync(ToDoCategoryCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name cannot be empty.");
            }

            ToDoCategory category = Mapper.Map<ToDoCategory>(dto);
            ToDoCategory result = await ToDoCategoryRepository.CreateToDoCategoryAsync(category);
            return Mapper.Map<ToDoCategoryResponseDTO>(result);
        }

        public async Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdAsync(Guid categoryId)
        {
            ToDoCategory? category = await ToDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }
            return Mapper.Map<ToDoCategoryResponseDTO>(category);
        }

        public async Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId)
        {
            ToDoCategory? category = await ToDoCategoryRepository.GetToDoCategoryByIdIncludeTasksAsync(categoryId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }
            return Mapper.Map<ToDoCategoryResponseDTO>(category);
        }

        public async Task<IEnumerable<ToDoCategoryResponseDTO>> GetAllToDoCategoriesAsync()
        {
            var categories = await ToDoCategoryRepository.GetAllToDoCategoriesAsync();
            return Mapper.Map<IEnumerable<ToDoCategoryResponseDTO>>(categories);
        }

        public async Task<ToDoCategoryResponseDTO> UpdateToDoCategoryAsync(ToDoCategoryUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name cannot be empty.");
            }

            ToDoCategory? existingCategory = await ToDoCategoryRepository.GetToDoCategoryByIdAsync(dto.Id);
            if (existingCategory is null)
            {
                throw new Exception($"ToDoCategory by id {dto.Id} does not found to update.");
            }

            ToDoCategory category = Mapper.Map<ToDoCategory>(dto);
            ToDoCategory result = await ToDoCategoryRepository.UpdateToDoCategoryAsync(category);
            return Mapper.Map<ToDoCategoryResponseDTO>(result);
        }

        public async Task<ToDoCategoryResponseDTO> DeleteToDoCategoryAsync(Guid categoryId)
        {
            ToDoCategory? category = await ToDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }

            ToDoCategory result = await ToDoCategoryRepository.DeleteToDoCategoryAsync(category);
            return Mapper.Map<ToDoCategoryResponseDTO>(result);
        }
    }
}
