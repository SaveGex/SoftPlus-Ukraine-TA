using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addingassets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "ToDoCategories");

            migrationBuilder.AddColumn<Guid>(
                name: "IconId",
                table: "ToDoCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Icons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoCategories_IconId",
                table: "ToDoCategories",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoCategories_Icons_IconId",
                table: "ToDoCategories",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoCategories_Icons_IconId",
                table: "ToDoCategories");

            migrationBuilder.DropTable(
                name: "Icons");

            migrationBuilder.DropIndex(
                name: "IX_ToDoCategories_IconId",
                table: "ToDoCategories");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "ToDoCategories");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "ToDoCategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
