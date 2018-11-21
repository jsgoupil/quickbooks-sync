using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Sample.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QbKvpStates",
                columns: table => new
                {
                    Ticket = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    CurrentStep = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QbKvpStates", x => new { x.Ticket, x.Key, x.CurrentStep });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QbTickets",
                columns: table => new
                {
                    Ticket = table.Column<string>(nullable: false),
                    CurrentStep = table.Column<string>(nullable: true),
                    Authenticated = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QbTickets", x => x.Ticket);
                    table.ForeignKey(
                        name: "FK_QbTickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "UserName" },
                values: new object[] { 1, "password", "jsgoupil" });

            migrationBuilder.CreateIndex(
                name: "IX_QbTickets_UserId",
                table: "QbTickets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QbKvpStates");

            migrationBuilder.DropTable(
                name: "QbTickets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
