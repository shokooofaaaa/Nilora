using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nilora.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackClientIdentifierAndSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientIdentifierHash",
                table: "Feedbacks",
                type: "varchar(64)",
                unicode: false,
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TechnicianId_ClientIdentifierHash_CreatedAt",
                table: "Feedbacks",
                columns: new[] { "TechnicianId", "ClientIdentifierHash", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_TechnicianId_ClientIdentifierHash_CreatedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ClientIdentifierHash",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Feedbacks");
        }
    }
}
