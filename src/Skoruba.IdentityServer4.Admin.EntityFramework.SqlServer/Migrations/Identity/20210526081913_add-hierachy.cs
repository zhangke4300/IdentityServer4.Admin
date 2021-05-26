using Microsoft.EntityFrameworkCore.Migrations;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Migrations.Identity
{
    public partial class addhierachy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HierarchyBases",
                columns: table => new
                {
                    DataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HierarchyBases", x => x.DataId);
                    table.ForeignKey(
                        name: "FK_HierarchyBases_HierarchyBases_ParentId",
                        column: x => x.ParentId,
                        principalTable: "HierarchyBases",
                        principalColumn: "DataId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HierarchyBases_ParentId",
                table: "HierarchyBases",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HierarchyBases");
        }
    }
}
