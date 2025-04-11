using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.API.Migrations
{
    /// <inheritdoc />
    public partial class modify_table_LoginHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "user",
                table: "LoginHistory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "user",
                table: "LoginHistory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "user",
                table: "LoginHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "user",
                table: "LoginHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                schema: "user",
                table: "LoginHistory");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                schema: "user",
                table: "LoginHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                schema: "user",
                table: "LoginHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "user",
                table: "LoginHistory",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "user",
                table: "LoginHistory");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                schema: "user",
                table: "LoginHistory",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                schema: "user",
                table: "LoginHistory",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "user",
                table: "LoginHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "user",
                table: "LoginHistory",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "user",
                table: "LoginHistory",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                schema: "user",
                table: "LoginHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                schema: "user",
                table: "LoginHistory",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
