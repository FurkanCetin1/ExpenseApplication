using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApplication.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseForm_Description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExpenseForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExpenseForms");
        }
    }
}
