using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApplication.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__8AFACE1A0B3BF962", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC5B1EBF7E", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK__Users__ManagerID__38996AB5",
                        column: x => x.ManagerID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

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
                        name: "FK__ExpenseAc__UserI__45F365D3",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseForms",
                columns: table => new
                {
                    ExpenseFormID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExpenseF__C5ECDAF530C9669A", x => x.ExpenseFormID);
                    table.ForeignKey(
                        name: "FK__ExpenseFo__Statu__6754599E",
                        column: x => x.StatusID,
                        principalTable: "ExpenseActions",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK__ExpenseFo__UserI__3E52440B",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseHistory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseFormID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExpenseH__3214EC27256B1F09", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExpenseHistory_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ExpenseActions",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK__ExpenseHi__Expen__5FB337D6",
                        column: x => x.ExpenseFormID,
                        principalTable: "ExpenseForms",
                        principalColumn: "ExpenseFormID");
                    table.ForeignKey(
                        name: "FK__ExpenseHi__UserI__619B8048",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReasonRejection = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpenseFormID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Expenses__1445CFF3939CDAE8", x => x.ExpenseID);
                    table.ForeignKey(
                        name: "FK__Expenses__Expens__6383C8BA",
                        column: x => x.ExpenseFormID,
                        principalTable: "ExpenseForms",
                        principalColumn: "ExpenseFormID");
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

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseForms_StatusID",
                table: "ExpenseForms",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseForms_UserID",
                table: "ExpenseForms",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseHistory_ExpenseFormID",
                table: "ExpenseHistory",
                column: "ExpenseFormID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseHistory_StatusID",
                table: "ExpenseHistory",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseHistory_UserID",
                table: "ExpenseHistory",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseFormID",
                table: "Expenses",
                column: "ExpenseFormID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerID",
                table: "Users",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__536C85E41B3C797F",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ExpenseAc__Expen__44FF419A",
                table: "ExpenseActions",
                column: "ExpenseID",
                principalTable: "Expenses",
                principalColumn: "ExpenseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ExpenseAc__Expen__44FF419A",
                table: "ExpenseActions");

            migrationBuilder.DropTable(
                name: "ExpenseHistory");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseForms");

            migrationBuilder.DropTable(
                name: "ExpenseActions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
