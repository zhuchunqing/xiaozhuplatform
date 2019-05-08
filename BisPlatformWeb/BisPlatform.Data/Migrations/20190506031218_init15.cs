using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BisPlatform.Data.Migrations
{
    public partial class init15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "xiaoma",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Score = table.Column<string>(maxLength: 12, nullable: false),
                    Age = table.Column<int>(nullable: false),
                    testtw = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xiaoma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "xiaoma2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BIM = table.Column<float>(maxLength: 12, nullable: false),
                    Height = table.Column<float>(maxLength: 12, nullable: false),
                    TestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xiaoma2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_xiaoma2_xiaoma_TestId",
                        column: x => x.TestId,
                        principalTable: "xiaoma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_xiaoma2_TestId",
                table: "xiaoma2",
                column: "TestId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "xiaoma2");

            migrationBuilder.DropTable(
                name: "xiaoma");
        }
    }
}
