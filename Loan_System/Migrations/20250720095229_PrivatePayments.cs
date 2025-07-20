using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_System.Migrations
{
    /// <inheritdoc />
    public partial class PrivatePayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateMoneyPaid",
                table: "ContractTable");

            migrationBuilder.CreateTable(
                name: "PrivateMoneyPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMoneyPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateMoneyPayments_ContractTable_ContractId",
                        column: x => x.ContractId,
                        principalTable: "ContractTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMoneyPayments_ContractId",
                table: "PrivateMoneyPayments",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateMoneyPayments");

            migrationBuilder.AddColumn<decimal>(
                name: "PrivateMoneyPaid",
                table: "ContractTable",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
