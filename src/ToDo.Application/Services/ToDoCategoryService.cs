using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class ToDoCategoryService : IToDoCategoryService
    {
        private readonly IToDoCategoryRepository _toDoCategoryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ToDoCategoryService(IToDoCategoryRepository toDoCategoryRepository,
            ICurrentUserService currentUserService,
            IAssetsRepository assetsRepository,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _toDoCategoryRepository = toDoCategoryRepository;
            _currentUserService = currentUserService;
            _assetsRepository = assetsRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ToDoCategoryResponseDTO> CreateToDoCategoryAsync(ToDoCategoryCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name cannot be empty.");
            }

            var category = dto.Adapt<ToDoCategory>();
            category.AuthorId = _currentUserService.UserId;

            category.Icon = await HandleIconUploadAsync(dto.Icon);

            try
            {
                var result = await _toDoCategoryRepository.CreateToDoCategoryAsync(category);
                await _unitOfWorkRepository.SaveChangesAsync();

                return MapToResponseDTO(result);
            }
            catch (Exception)
            {
                HandleIconFileDelete(category.Icon);
                throw;
            }
        }


        public async Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdAsync(Guid categoryId)
        {
            var category = await _toDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId, _currentUserService.UserId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }

            return MapToResponseDTO(category);
        }

        public async Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId)
        {
            var category = await _toDoCategoryRepository.GetToDoCategoryByIdIncludeTasksAsync(categoryId, _currentUserService.UserId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }

            return MapToResponseDTO(category);
        }

        public async Task<IEnumerable<ToDoCategoryResponseDTO>> GetAllToDoCategoriesAsync()
        {
            var categories = await _toDoCategoryRepository.GetAllToDoCategoriesAsync(_currentUserService.UserId);

            return categories.Select(MapToResponseDTO);
        }

        public async Task<ToDoCategoryResponseDTO> UpdateToDoCategoryAsync(Guid categoryId, ToDoCategoryUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name cannot be empty.");
            }

            var existingCategory = await _toDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId, _currentUserService.UserId);
            if (existingCategory is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found to update.");
            }

            string? oldIconName = existingCategory.Icon?.Name;
            Icon? newIconEntity = null;

            if (dto.Icon != null)
            {
                newIconEntity = await HandleIconUploadAsync(dto.Icon);
            }

            dto.Adapt(existingCategory);

            if (dto.Icon != null)
            {
                existingCategory.Icon = newIconEntity;
            }

            try
            {
                var result = await _toDoCategoryRepository.UpdateToDoCategoryAsync(existingCategory);
                await _unitOfWorkRepository.SaveChangesAsync();

                if (dto.Icon != null && oldIconName != null)
                {
                    _assetsRepository.DeletePhysicalFile(oldIconName);
                }

                return MapToResponseDTO(result);
            }
            catch (Exception)
            {
                HandleIconFileDelete(newIconEntity);
                throw;
            }
        }

        public async Task<ToDoCategoryResponseDTO> DeleteToDoCategoryAsync(Guid categoryId)
        {
            var category = await _toDoCategoryRepository.GetToDoCategoryByIdAsync(categoryId, _currentUserService.UserId);
            if (category is null)
            {
                throw new Exception($"ToDoCategory by id {categoryId} does not found.");
            }

            var iconToDelete = category.Icon;

            var result = await _toDoCategoryRepository.DeleteToDoCategoryAsync(category);
            await _unitOfWorkRepository.SaveChangesAsync();

            HandleIconFileDelete(iconToDelete);

            return MapToResponseDTO(result);
        }
        #region helpers
        private ToDoCategoryResponseDTO MapToResponseDTO(ToDoCategory category)
        {
            var iconUrl = category.Icon != null
                ? _assetsRepository.GetAssetUrl(category.Icon.Name)
                : null;

            var tasksDto = category.Tasks != null
                ? category.Tasks.Adapt<List<ToDoTaskResponseDTO>>()
                : new List<ToDoTaskResponseDTO>();

            return new ToDoCategoryResponseDTO(
                category.Id,
                category.Name,
                iconUrl,
                tasksDto
            );
        }

        private async Task<Icon?> HandleIconUploadAsync(IFormFile? iconFile)
        {
            if (iconFile is null) return null;

            var extension = Path.GetExtension(iconFile.FileName);
            using var stream = iconFile.OpenReadStream();

            var savedIcon = await _assetsRepository.SaveAssetAsync(stream, extension);

            return savedIcon;
        }

        private void HandleIconFileDelete(Icon? icon)
        {
            if (icon != null && !string.IsNullOrWhiteSpace(icon.Name))
            {
                _assetsRepository.DeletePhysicalFile(icon.Name);
            }
        }

        #endregion
    }
}
