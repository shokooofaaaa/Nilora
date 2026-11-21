using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nilora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_RejectionReason_To_Feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Feedbacks");
        }
    }
}
