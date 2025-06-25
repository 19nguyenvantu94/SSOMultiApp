using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authen.Data.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AddIdClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Identity",
                table: "ClientClaimPolicyRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Identity",
                table: "ClientClaimPolicyRoles");
        }
    }
}
