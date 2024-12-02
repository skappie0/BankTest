using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTest.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountDebitId = table.Column<int>(type: "int", nullable: false),
                    AccountCreditId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountCreditId",
                        column: x => x.AccountCreditId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountDebitId",
                        column: x => x.AccountDebitId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountCreditId",
                table: "Transactions",
                column: "AccountCreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountDebitId",
                table: "Transactions",
                column: "AccountDebitId");

            migrationBuilder.Sql(
            @"INSERT INTO Transactions (AccountDebitId, AccountCreditId, Amount, Text)
              VALUES (1, 2, 100, 'Top up User1 account');");
           /* migrationBuilder.InsertData(
                       table: "Transactions",
                       columns: new[] { "AccountDebitId", "AccountCreditId", "Amount" },
                       values: new object[] { 2, "22221111", "User1 account" }
                   );*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
