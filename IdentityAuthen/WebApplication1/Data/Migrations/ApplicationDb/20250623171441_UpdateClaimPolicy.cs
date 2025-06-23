using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authen.Data.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class UpdateClaimPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimValue",
                schema: "Identity",
                table: "ClientClaimPolicies");

            migrationBuilder.DropColumn(
                name: "ClientId",
                schema: "Identity",
                table: "ClientClaimPolicies");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                schema: "Identity",
                table: "ClientClaimPolicies");

            migrationBuilder.DropColumn(
                name: "RequiredClaim",
                schema: "Identity",
                table: "ClientClaimPolicies");

            migrationBuilder.AddColumn<int>(
                name: "IdClient",
                schema: "Identity",
                table: "ClientClaimPolicies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClientClaimPolicyRoles",
                schema: "Identity",
                columns: table => new
                {
                    ClientClaimPolicyId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClaimPolicyRoles", x => new { x.ClientClaimPolicyId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ClientClaimPolicyRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientClaimPolicyRoles_ClientClaimPolicies_ClientClaimPolicy~",
                        column: x => x.ClientClaimPolicyId,
                        principalSchema: "Identity",
                        principalTable: "ClientClaimPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClaimPolicyRoles_RoleId",
                schema: "Identity",
                table: "ClientClaimPolicyRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientClaimPolicyRoles",
                schema: "Identity");

            migrationBuilder.DropColumn(
                name: "IdClient",
                schema: "Identity",
                table: "ClientClaimPolicies");

            migrationBuilder.AddColumn<bool>(
                name: "ClaimValue",
                schema: "Identity",
                table: "ClientClaimPolicies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                schema: "Identity",
                table: "ClientClaimPolicies",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                schema: "Identity",
                table: "ClientClaimPolicies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequiredClaim",
                schema: "Identity",
                table: "ClientClaimPolicies",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
