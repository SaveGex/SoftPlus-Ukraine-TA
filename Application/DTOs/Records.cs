using Domain.Enums;

namespace Application.DTOs
{
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
        string? Icon
    );

    public record ToDoCategoryUpdateDTO(
        string Name,
        string? Icon
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
}