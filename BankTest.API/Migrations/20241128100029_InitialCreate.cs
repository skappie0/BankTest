using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTest.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            // Insert a default bank user
            migrationBuilder.InsertData(
                       table: "Users",
                       columns: new[] { "Login", "Password", "Name", "EMail", "Address" },
                       values: new object[] { "bank", "bank111", "Bank test User", "bank@bank.com", "Bank Test Street" }
                   );
            // Insert a default bank user
            migrationBuilder.InsertData(
                       table: "Users",
                       columns: new[] { "Login", "Password", "Name", "EMail", "Address" },
                       values: new object[] { "user1", "user111", "Test User", "user1@bank.com", "User Test Street" }
                   );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
