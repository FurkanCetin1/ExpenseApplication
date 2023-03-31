using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApplication.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpenseHistoryId",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseHistoryId",
                table: "Expenses",
                column: "ExpenseHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseHistory_ExpenseHistoryId",
                table: "Expenses",
                column: "ExpenseHistoryId",
                principalTable: "ExpenseHistory",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseHistory_ExpenseHistoryId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpenseHistoryId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseHistoryId",
                table: "Expenses");
        }
    }
}
