using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ThunderPay.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init_Thunderpay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "thunderpay");

            migrationBuilder.CreateTable(
                name: "organizations",
                schema: "thunderpay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "merchants",
                schema: "thunderpay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    UniqueId = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    OrganizationDbmId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_merchants_organizations_OrganizationDbmId",
                        column: x => x.OrganizationDbmId,
                        principalSchema: "thunderpay",
                        principalTable: "organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_merchants_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "thunderpay",
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_merchants_OrganizationDbmId",
                schema: "thunderpay",
                table: "merchants",
                column: "OrganizationDbmId");

            migrationBuilder.CreateIndex(
                name: "IX_merchants_OrganizationId_DisplayName",
                schema: "thunderpay",
                table: "merchants",
                columns: new[] { "OrganizationId", "DisplayName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_merchants_UniqueId",
                schema: "thunderpay",
                table: "merchants",
                column: "UniqueId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "merchants",
                schema: "thunderpay");

            migrationBuilder.DropTable(
                name: "organizations",
                schema: "thunderpay");
        }
    }
}
