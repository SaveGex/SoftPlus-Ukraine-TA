using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Models;
using Domain.Repositories.Interfaces;
using Mapster;
using MapsterMapper;

namespace Application.Services
{
    public class ToDoCategoryService : IToDoCategoryService
    {
        private readonly IToDoCategoryRepository _toDoCategoryRepository;

        public ToDoCategoryService(IToDoCategoryRepository toDoCategoryRepository)
        {
            _toDoCategoryRepository = toDoCategoryRepository;
        }

        public async Task<ToDoCategoryResponseDTO> CreateToDoCategoryAsync(ToDoCategoryCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name cannot be empty.");
            }

            var category = dto.Adapt<ToDoCategory>();
            var result = await _toDoCategoryRepository.CreateToDoCategoryAsync(category);

            return result.Adapt<ToDoCategoryResponseDTO>();
        }

        public async Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdAsync(Guid categoryId)
        {
            var category = await _toDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }

            return category.Adapt<ToDoCategoryResponseDTO>();
        }

        public async Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId)
        {
            var category = await _toDoCategoryRepository.GetToDoCategoryByIdIncludeTasksAsync(categoryId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }

            return category.Adapt<ToDoCategoryResponseDTO>();
        }

        public async Task<IEnumerable<ToDoCategoryResponseDTO>> GetAllToDoCategoriesAsync()
        {
            var categories = await _toDoCategoryRepository.GetAllToDoCategoriesAsync();

            return categories.Adapt<IEnumerable<ToDoCategoryResponseDTO>>();
        }

        public async Task<ToDoCategoryResponseDTO> UpdateToDoCategoryAsync(Guid categoryId, ToDoCategoryUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name cannot be empty.");
            }

            var existingCategory = await _toDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId);
            if (existingCategory is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found to update.");
            }

            dto.Adapt(existingCategory);
            var result = await _toDoCategoryRepository.UpdateToDoCategoryAsync(existingCategory);

            return result.Adapt<ToDoCategoryResponseDTO>();
        }

        public async Task<ToDoCategoryResponseDTO> DeleteToDoCategoryAsync(Guid categoryId)
        {
            var category = await _toDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }

            var result = await _toDoCategoryRepository.DeleteToDoCategoryAsync(category);

            return result.Adapt<ToDoCategoryResponseDTO>();
        }
    }
}
