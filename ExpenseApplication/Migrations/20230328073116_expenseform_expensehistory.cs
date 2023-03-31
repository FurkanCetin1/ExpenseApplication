using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApplication.Migrations
{
    /// <inheritdoc />
    public partial class expenseform_expensehistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseForms_Statuses_StatusID",
                table: "ExpenseForms");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHistory_Statuses_StatusID",
                table: "ExpenseHistory");

            migrationBuilder.AlterColumn<int>(
                name: "StatusID",
                table: "ExpenseHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StatusID",
                table: "ExpenseForms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseForms_Statuses_StatusID",
                table: "ExpenseForms",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseHistory_Statuses_StatusID",
                table: "ExpenseHistory",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseForms_Statuses_StatusID",
                table: "ExpenseForms");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHistory_Statuses_StatusID",
                table: "ExpenseHistory");

            migrationBuilder.AlterColumn<int>(
                name: "StatusID",
                table: "ExpenseHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusID",
                table: "ExpenseForms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseForms_Statuses_StatusID",
                table: "ExpenseForms",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseHistory_Statuses_StatusID",
                table: "ExpenseHistory",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
