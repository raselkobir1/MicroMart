using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.CreateTable(
                name: "InventoryInfos",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryHistory",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryInfoId = table.Column<long>(type: "bigint", nullable: false),
                    SKU = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    QuentityChanged = table.Column<int>(type: "integer", nullable: false),
                    LastQuentity = table.Column<int>(type: "integer", nullable: false),
                    NewQuentity = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryHistory_InventoryInfos_InventoryInfoId",
                        column: x => x.InventoryInfoId,
                        principalSchema: "inventory",
                        principalTable: "InventoryInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryHistory_InventoryInfoId",
                schema: "inventory",
                table: "InventoryHistory",
                column: "InventoryInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryHistory",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "InventoryInfos",
                schema: "inventory");
        }
    }
}
