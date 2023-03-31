using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApplication.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ExpenseFo__Statu__6754599E",
                table: "ExpenseForms");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHistory_StatusID",
                table: "ExpenseHistory");

            migrationBuilder.DropTable(
                name: "ExpenseActions");

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

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseForms_Statuses_StatusID",
                table: "ExpenseForms");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHistory_Statuses_StatusID",
                table: "ExpenseHistory");

            migrationBuilder.DropTable(
                name: "Statuses");

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

            migrationBuilder.CreateTable(
                name: "ExpenseActions",
                columns: table => new
                {
                    ExpenseActionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExpenseA__4C4C1B723FD4F5E1", x => x.ExpenseActionID);
                    table.UniqueConstraint("AK_ExpenseActions_StatusID", x => x.StatusID);
                    table.ForeignKey(
                        name: "FK__ExpenseAc__Expen__44FF419A",
                        column: x => x.ExpenseID,
                        principalTable: "Expenses",
                        principalColumn: "ExpenseID");
                    table.ForeignKey(
                        name: "FK__ExpenseAc__UserI__45F365D3",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseActions_ExpenseID",
                table: "ExpenseActions",
                column: "ExpenseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseActions_UserID",
                table: "ExpenseActions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UQ_StatusID",
                table: "ExpenseActions",
                column: "StatusID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ExpenseFo__Statu__6754599E",
                table: "ExpenseForms",
                column: "StatusID",
                principalTable: "ExpenseActions",
                principalColumn: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseHistory_StatusID",
                table: "ExpenseHistory",
                column: "StatusID",
                principalTable: "ExpenseActions",
                principalColumn: "StatusID");
        }
    }
}
