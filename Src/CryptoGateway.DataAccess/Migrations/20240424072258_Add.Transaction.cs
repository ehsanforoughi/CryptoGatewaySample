using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "Payment",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    CurrencyType = table.Column<byte>(type: "tinyint", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    ReferenceName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ReferenceNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    ActionType = table.Column<byte>(type: "tinyint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserCreditId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_UserCredit_UserCreditId",
                        column: x => x.UserCreditId,
                        principalSchema: "Base",
                        principalTable: "UserCredit",
                        principalColumn: "UserCreditId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserCreditId",
                schema: "Payment",
                table: "Transaction",
                column: "UserCreditId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "Payment");
        }
    }
}
