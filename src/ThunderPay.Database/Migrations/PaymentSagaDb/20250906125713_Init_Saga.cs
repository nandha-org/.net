using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThunderPay.Database.Migrations.PaymentSagaDb
{
    /// <inheritdoc />
    public partial class Init_Saga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "saga");

            migrationBuilder.CreateTable(
                name: "eftSubmissionSagaState",
                schema: "saga",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentState = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eftSubmissionSagaState", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eftSubmissionSagaState",
                schema: "saga");
        }
    }
}
