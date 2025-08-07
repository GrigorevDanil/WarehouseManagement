using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OutcomeProcessing_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "outcome-processing");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "outcome-processing",
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
                name: "OutcomeDocuments",
                schema: "outcome-processing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumDocument = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutcomeDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutcomeResources",
                schema: "outcome-processing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    OutcomeDocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceQuantity = table.Column<int>(type: "integer", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutcomeResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutcomeResources_OutcomeDocuments_OutcomeDocumentId",
                        column: x => x.OutcomeDocumentId,
                        principalSchema: "outcome-processing",
                        principalTable: "OutcomeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_outbox_messages_unprocessed",
                schema: "outcome-processing",
                table: "OutboxMessages",
                columns: new[] { "CreatedAt", "ProcessedAt" },
                filter: "\"ProcessedAt\" IS NULL")
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Type", "Payload" });

            migrationBuilder.CreateIndex(
                name: "IX_OutcomeResources_OutcomeDocumentId",
                schema: "outcome-processing",
                table: "OutcomeResources",
                column: "OutcomeDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "outcome-processing");

            migrationBuilder.DropTable(
                name: "OutcomeResources",
                schema: "outcome-processing");

            migrationBuilder.DropTable(
                name: "OutcomeDocuments",
                schema: "outcome-processing");
        }
    }
}
