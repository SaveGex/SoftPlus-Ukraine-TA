using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changednames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoSteps_TodoTasks_TodoTaskId",
                table: "TodoSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_TodoCategories_CategoryId",
                table: "TodoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTasks",
                table: "TodoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoSteps",
                table: "TodoSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoCategories",
                table: "TodoCategories");

            migrationBuilder.RenameTable(
                name: "TodoTasks",
                newName: "ToDoTasks");

            migrationBuilder.RenameTable(
                name: "TodoSteps",
                newName: "ToDoSteps");

            migrationBuilder.RenameTable(
                name: "TodoCategories",
                newName: "ToDoCategories");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTasks_CategoryId",
                table: "ToDoTasks",
                newName: "IX_ToDoTasks_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoSteps_TodoTaskId",
                table: "ToDoSteps",
                newName: "IX_ToDoSteps_TodoTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDoTasks",
                table: "ToDoTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDoSteps",
                table: "ToDoSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDoCategories",
                table: "ToDoCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoSteps_ToDoTasks_TodoTaskId",
                table: "ToDoSteps",
                column: "TodoTaskId",
                principalTable: "ToDoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoTasks_ToDoCategories_CategoryId",
                table: "ToDoTasks",
                column: "CategoryId",
                principalTable: "ToDoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoSteps_ToDoTasks_TodoTaskId",
                table: "ToDoSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoTasks_ToDoCategories_CategoryId",
                table: "ToDoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDoTasks",
                table: "ToDoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDoSteps",
                table: "ToDoSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDoCategories",
                table: "ToDoCategories");

            migrationBuilder.RenameTable(
                name: "ToDoTasks",
                newName: "TodoTasks");

            migrationBuilder.RenameTable(
                name: "ToDoSteps",
                newName: "TodoSteps");

            migrationBuilder.RenameTable(
                name: "ToDoCategories",
                newName: "TodoCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoTasks_CategoryId",
                table: "TodoTasks",
                newName: "IX_TodoTasks_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoSteps_TodoTaskId",
                table: "TodoSteps",
                newName: "IX_TodoSteps_TodoTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTasks",
                table: "TodoTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoSteps",
                table: "TodoSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoCategories",
                table: "TodoCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoSteps_TodoTasks_TodoTaskId",
                table: "TodoSteps",
                column: "TodoTaskId",
                principalTable: "TodoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_TodoCategories_CategoryId",
                table: "TodoTasks",
                column: "CategoryId",
                principalTable: "TodoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
