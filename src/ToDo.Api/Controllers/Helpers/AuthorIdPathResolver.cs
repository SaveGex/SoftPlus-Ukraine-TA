namespace To_Do_application.Controllers.Helpers
{
    internal static class AuthorIdPathResolver
    {

        internal const string StepAuthorIdPath =
                    nameof(Domain.Models.ToDoStep) + "." +
                    nameof(Domain.Models.ToDoStep.TodoTask) + "." +
                    nameof(Domain.Models.ToDoStep.TodoTask.AuthorId);
        internal const string TaskAuthorIdPath = 
            nameof(Domain.Models.ToDoTask) + "." + 
            nameof(Domain.Models.ToDoTask.AuthorId);
    }
}
