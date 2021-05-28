using Microsoft.EntityFrameworkCore.Migrations;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Migrations.Identity
{
    public partial class AddDataKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataKey",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataKey",
                table: "Users");
        }
    }
}
