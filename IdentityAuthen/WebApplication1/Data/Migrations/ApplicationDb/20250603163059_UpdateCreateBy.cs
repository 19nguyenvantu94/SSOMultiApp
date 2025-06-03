using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authen.Data.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class UpdateCreateBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetRoleClaims_AspNetUsers_UserId",
            //    schema: "Identity",
            //    table: "AspNetRoleClaims");

            //migrationBuilder.DropIndex(
            //    name: "IX_AspNetRoleClaims_UserId",
            //    schema: "Identity",
            //    table: "AspNetRoleClaims");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    schema: "Identity",
            //    table: "AspNetRoleClaims");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "AspNetUserClaims",
                type: "nvarchar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "Identity",
                table: "AspNetUserClaims",
                type: "nvarchar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "AspNetUserClaims",
                keyColumn: "LastModifiedBy",
                keyValue: null,
                column: "LastModifiedBy",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "AspNetUserClaims",
                type: "nvarchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "AspNetUserClaims",
                keyColumn: "CreatedBy",
                keyValue: null,
                column: "CreatedBy",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "Identity",
                table: "AspNetUserClaims",
                type: "nvarchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldNullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "UserId",
            //    schema: "Identity",
            //    table: "AspNetRoleClaims",
            //    type: "char(36)",
            //    nullable: true,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            //    collation: "ascii_general_ci");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_UserId",
            //    schema: "Identity",
            //    table: "AspNetRoleClaims",
            //    column: "UserId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetRoleClaims_AspNetUsers_UserId",
            //    schema: "Identity",
            //    table: "AspNetRoleClaims",
            //    column: "UserId",
            //    principalSchema: "Identity",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
