using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    // --- USER DTOs ---
    public record RegisterRequestDTO(string Email, string Password);
    public record LoginRequestDTO(string Email, string Password);
    public record AuthResponseDTO(bool IsSuccess, string Token, string Message);

    // --- TASK DTOs ---
    public record ToDoTaskResponseDTO(
        Guid Id,
        string Title,
        string? Note,
        bool IsCompleted,
        bool IsImportant,
        bool IsMyDay,
        DateTime? ReminderAt,
        DateTime? DueDate,
        DateTime CreatedAt,
        RecurrenceType Recurrence,
        Guid? CategoryId,
        Guid UserId,
        List<ToDoStepResponseDTO> Steps
    );

    public record ToDoTaskCreateDTO(
        string Title,
        string? Note,
        bool IsImportant,
        bool IsMyDay,
        DateTime? ReminderAt,
        DateTime? DueDate,
        RecurrenceType Recurrence,
        Guid? CategoryId
    );

    public record ToDoTaskUpdateDTO(
        string Title,
        string? Note,
        bool IsCompleted,
        bool IsImportant,
        bool IsMyDay,
        DateTime? ReminderAt,
        DateTime? DueDate,
        RecurrenceType Recurrence,
        Guid? CategoryId
    );

    // --- CATEGORY DTOs ---
    public record ToDoCategoryResponseDTO(
        Guid Id,
        string Name,
        string? Icon,
        List<ToDoTaskResponseDTO> Tasks
    );

    public record ToDoCategoryCreateDTO(
        string Name,
        IFormFile? Icon
    );

    public record ToDoCategoryUpdateDTO(
        string Name,
        IFormFile? Icon
    );

    // --- STEP DTOs ---
    public record ToDoStepResponseDTO(
        Guid Id,
        string Title,
        bool IsCompleted,
        Guid TodoTaskId
    );

    public record ToDoStepCreateDTO(
        string Title,
        Guid TodoTaskId
    );

    public record ToDoStepUpdateDTO(
        string Title,
        bool IsCompleted
    );

    // --- ICON DTOs ---
    public record IconResponseDTO(
        string Id,
        string IconUrl
    );

    public record IconCreateDTO(
        IFormFile File
    );
}