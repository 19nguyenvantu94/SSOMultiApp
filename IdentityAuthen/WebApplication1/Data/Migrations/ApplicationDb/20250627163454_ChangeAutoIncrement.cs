using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authen.Data.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class ChangeAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientClaimPolicyRoles",
                schema: "Identity",
                table: "ClientClaimPolicyRoles");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Identity",
                table: "ClientClaimPolicyRoles",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientClaimPolicyRoles",
                schema: "Identity",
                table: "ClientClaimPolicyRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClaimPolicyRoles_ClientClaimPolicyId_RoleId",
                schema: "Identity",
                table: "ClientClaimPolicyRoles",
                columns: new[] { "ClientClaimPolicyId", "RoleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientClaimPolicyRoles",
                schema: "Identity",
                table: "ClientClaimPolicyRoles");

            migrationBuilder.DropIndex(
                name: "IX_ClientClaimPolicyRoles_ClientClaimPolicyId_RoleId",
                schema: "Identity",
                table: "ClientClaimPolicyRoles");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Identity",
                table: "ClientClaimPolicyRoles",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientClaimPolicyRoles",
                schema: "Identity",
                table: "ClientClaimPolicyRoles",
                columns: new[] { "ClientClaimPolicyId", "RoleId" });
        }
    }
}
