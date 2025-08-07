using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagement.BalanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BalanceManagement_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "balance-management");

            migrationBuilder.CreateTable(
                name: "Balances",
                schema: "balance-management",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceStock = table.Column<int>(type: "integer", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balances",
                schema: "balance-management");
        }
    }
}
