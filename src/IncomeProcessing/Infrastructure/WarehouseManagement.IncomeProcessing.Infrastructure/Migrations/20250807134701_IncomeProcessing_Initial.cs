using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncomeProcessing_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "income-processing");

            migrationBuilder.CreateTable(
                name: "IncomeDocuments",
                schema: "income-processing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumDocument = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "income-processing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Payload = table.Column<string>(type: "jsonb", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeResources",
                schema: "income-processing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    IncomeDocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceQuantity = table.Column<int>(type: "integer", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeResources_IncomeDocuments_IncomeDocumentId",
                        column: x => x.IncomeDocumentId,
                        principalSchema: "income-processing",
                        principalTable: "IncomeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeResources_IncomeDocumentId",
                schema: "income-processing",
                table: "IncomeResources",
                column: "IncomeDocumentId");

            migrationBuilder.CreateIndex(
                name: "idx_outbox_messages_unprocessed",
                schema: "income-processing",
                table: "OutboxMessages",
                columns: new[] { "CreatedAt", "ProcessedAt" },
                filter: "\"ProcessedAt\" IS NULL")
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Type", "Payload" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeResources",
                schema: "income-processing");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "income-processing");

            migrationBuilder.DropTable(
                name: "IncomeDocuments",
                schema: "income-processing");
        }
    }
}
