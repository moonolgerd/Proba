using Microsoft.EntityFrameworkCore.Migrations;

namespace Proba.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Probas",
                columns: table => new
                {
                    Greeting = table.Column<string>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Probas", x => x.Greeting);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Probas");
        }
    }
}
