using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTest.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                        name: "Accounts",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "int", nullable: false)
                                .Annotation("SqlServer:Identity", "1, 1"),
                            UserId = table.Column<int>(type: "int", nullable: false),
                            AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                            Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Accounts", x => x.Id);

                            // Add Foreign Key Constraint
                            table.ForeignKey(
                                name: "FK_Accounts_Users_UserId",
                                column: x => x.UserId,
                                principalTable: "Users",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade); // Optional: Cascade delete
                        });

            // Insert a default bank account
            migrationBuilder.InsertData(
                       table: "Accounts",
                       columns: new[] { "UserId", "AccountNumber", "Description" },
                       values: new object[] { 1, "11111111", "Bank cash account" }
                   );
            // Insert a default user account
            migrationBuilder.InsertData(
                       table: "Accounts",
                       columns: new[] { "UserId", "AccountNumber", "Description" },
                       values: new object[] { 2, "22221111", "User1 account" }
                   );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                        name: "Accounts");
        }
    }
}
