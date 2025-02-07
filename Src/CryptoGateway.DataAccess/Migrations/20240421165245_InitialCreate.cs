using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.EnsureSchema(
                name: "Payment");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "ContractAccount",
                schema: "Payment",
                columns: table => new
                {
                    ContractAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressBase58 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressHex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAccount", x => x.ContractAccountId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "Base",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    FullName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NameFa = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsFiat = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    DecimalPlaces = table.Column<byte>(type: "tinyint", nullable: false),
                    IsTradable = table.Column<bool>(type: "bit", nullable: false),
                    IsDepositable = table.Column<bool>(type: "bit", nullable: false),
                    IsWithdrawable = table.Column<bool>(type: "bit", nullable: false),
                    NetworkTransferFee = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    MinimumAmount = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    MinimumDeposit = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    MinimumWithdraw = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                schema: "Auth",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    OrderPriority = table.Column<byte>(type: "tinyint", nullable: false),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Icon = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Path = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_Menu_Menu_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Auth",
                        principalTable: "Menu",
                        principalColumn: "MenuId");
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RoleNameFa = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RoleNameEn = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    InsertDateMi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Base",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserExternalId = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: false),
                    MobileNumber = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "varchar(55)", maxLength: 55, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NationalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Salt = table.Column<string>(type: "char(8)", maxLength: 8, nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "MenuInRole",
                schema: "Auth",
                columns: table => new
                {
                    MenuInRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuInRole", x => x.MenuInRoleId);
                    table.ForeignKey(
                        name: "FK_MenuInRole_Menu_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "Auth",
                        principalTable: "Menu",
                        principalColumn: "MenuId");
                    table.ForeignKey(
                        name: "FK_MenuInRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRole",
                schema: "Auth",
                columns: table => new
                {
                    UserInRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRole", x => x.UserInRoleId);
                    table.ForeignKey(
                        name: "FK_UserInRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "Base",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    CardNumber = table.Column<string>(type: "varchar(19)", maxLength: 19, nullable: false),
                    Sheba = table.Column<string>(type: "varchar(26)", maxLength: 26, nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccount_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CustodyAccount",
                schema: "Payment",
                columns: table => new
                {
                    CustodyAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustodyAccExternalId = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractAccountId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustodyAccount", x => x.CustodyAccountId);
                    table.ForeignKey(
                        name: "FK_CustodyAccount_ContractAccount_ContractAccountId",
                        column: x => x.ContractAccountId,
                        principalSchema: "Payment",
                        principalTable: "ContractAccount",
                        principalColumn: "ContractAccountId");
                    table.ForeignKey(
                        name: "FK_CustodyAccount_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserCredit",
                schema: "Base",
                columns: table => new
                {
                    UserCreditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    CurrencyType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredit", x => x.UserCreditId);
                    table.ForeignKey(
                        name: "FK_UserCredit_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                schema: "Base",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Network = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Memo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Tag = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_Wallet_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Base",
                        principalTable: "Currency",
                        principalColumn: "CurrencyId");
                    table.ForeignKey(
                        name: "FK_Wallet_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PayIn",
                schema: "Payment",
                columns: table => new
                {
                    PayInId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    CurrencyType = table.Column<byte>(type: "tinyint", nullable: false),
                    TransferType = table.Column<byte>(type: "tinyint", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    CustodyAccountId = table.Column<int>(type: "int", nullable: false),
                    TxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayInDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayIn", x => x.PayInId);
                    table.ForeignKey(
                        name: "FK_PayIn_CustodyAccount_CustodyAccountId",
                        column: x => x.CustodyAccountId,
                        principalSchema: "Payment",
                        principalTable: "CustodyAccount",
                        principalColumn: "CustodyAccountId");
                    table.ForeignKey(
                        name: "FK_PayIn_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentExternalId = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(24,12)", nullable: false, defaultValue: 0m),
                    PriceCurrencyType = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    PayAmount = table.Column<decimal>(type: "decimal(24,12)", nullable: false, defaultValue: 0m),
                    PayCurrencyType = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    SpotUsdtPrice = table.Column<decimal>(type: "decimal(24,12)", nullable: false, defaultValue: 0m),
                    ComPercentage = table.Column<decimal>(type: "decimal(24,12)", nullable: false, defaultValue: 0m),
                    ComFixedValue = table.Column<decimal>(type: "decimal(24,12)", nullable: false, defaultValue: 0m),
                    ComCurrencyType = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    CallBackUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustodyAccountId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_CustodyAccount_CustodyAccountId",
                        column: x => x.CustodyAccountId,
                        principalSchema: "Payment",
                        principalTable: "CustodyAccount",
                        principalColumn: "CustodyAccountId");
                    table.ForeignKey(
                        name: "FK_Payment_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Payout",
                schema: "Payment",
                columns: table => new
                {
                    PayoutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(24,12)", nullable: false),
                    CurrencyType = table.Column<byte>(type: "tinyint", nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    TransferType = table.Column<byte>(type: "tinyint", nullable: false),
                    BankTrackingCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payout", x => x.PayoutId);
                    table.ForeignKey(
                        name: "FK_Payout_BankAccount_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "Base",
                        principalTable: "BankAccount",
                        principalColumn: "BankAccountId");
                    table.ForeignKey(
                        name: "FK_Payout_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Payout_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "Base",
                        principalTable: "Wallet",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_UserId",
                schema: "Base",
                table: "BankAccount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyAccount_ContractAccountId",
                schema: "Payment",
                table: "CustodyAccount",
                column: "ContractAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyAccount_CustodyAccExternalId",
                schema: "Payment",
                table: "CustodyAccount",
                column: "CustodyAccExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustodyAccount_UserId",
                schema: "Payment",
                table: "CustodyAccount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ParentId",
                schema: "Auth",
                table: "Menu",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuInRole_MenuId",
                schema: "Auth",
                table: "MenuInRole",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuInRole_RoleId_MenuId",
                schema: "Auth",
                table: "MenuInRole",
                columns: new[] { "RoleId", "MenuId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayIn_CustodyAccountId",
                schema: "Payment",
                table: "PayIn",
                column: "CustodyAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PayIn_UserId",
                schema: "Payment",
                table: "PayIn",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CustodyAccountId",
                schema: "Payment",
                table: "Payment",
                column: "CustodyAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentExternalId",
                schema: "Payment",
                table: "Payment",
                column: "PaymentExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                schema: "Payment",
                table: "Payment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payout_BankAccountId",
                schema: "Payment",
                table: "Payout",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payout_UserId",
                schema: "Payment",
                table: "Payout",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payout_WalletId",
                schema: "Payment",
                table: "Payout",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Base",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_MobileNumber",
                schema: "Base",
                table: "User",
                column: "MobileNumber",
                unique: true,
                filter: "[MobileNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserExternalId",
                schema: "Base",
                table: "User",
                column: "UserExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredit_UserId",
                schema: "Base",
                table: "UserCredit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRole_RoleId_UserId",
                schema: "Auth",
                table: "UserInRole",
                columns: new[] { "RoleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CurrencyId",
                schema: "Base",
                table: "Wallet",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                schema: "Base",
                table: "Wallet",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuInRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "PayIn",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Payout",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "UserCredit",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "UserInRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Menu",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "CustodyAccount",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Wallet",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ContractAccount",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Base");
        }
    }
}
