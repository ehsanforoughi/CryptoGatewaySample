using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddContractTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Contract");

            migrationBuilder.CreateTable(
                name: "ContractTransaction",
                schema: "Contract",
                columns: table => new
                {
                    ContractTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractAccountId = table.Column<int>(type: "int", nullable: false),
                    TxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractResource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Expiration = table.Column<long>(type: "bigint", nullable: false),
                    RefBlockBytes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeeLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnergyUsage = table.Column<int>(type: "int", nullable: false),
                    EnergyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GasLimit = table.Column<int>(type: "int", nullable: false),
                    GasPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BandwidthUsage = table.Column<int>(type: "int", nullable: false),
                    BandwidthFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTransaction", x => x.ContractTransactionId);
                    table.ForeignKey(
                        name: "FK_ContractTransaction_ContractAccount_ContractAccountId",
                        column: x => x.ContractAccountId,
                        principalSchema: "Payment",
                        principalTable: "ContractAccount",
                        principalColumn: "ContractAccountId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractTransaction_ContractAccountId",
                schema: "Contract",
                table: "ContractTransaction",
                column: "ContractAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractTransaction",
                schema: "Contract");
        }
    }
}
