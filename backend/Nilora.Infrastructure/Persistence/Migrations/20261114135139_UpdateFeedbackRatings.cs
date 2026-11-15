using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nilora.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedbackRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Feedbacks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<int>(
                name: "BehaviorRating",
                table: "Feedbacks",
                type: "int",
                nullable: false
                );

            migrationBuilder.AddColumn<int>(
                name: "DurabilityRating",
                table: "Feedbacks",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "HygieneRating",
                table: "Feedbacks",
                type: "int",
                nullable: false
                );

            migrationBuilder.AddColumn<int>(
                name: "QualityRating",
                table: "Feedbacks",
                type: "int",
                nullable: false
                );

            migrationBuilder.AddCheckConstraint(
                name: "CK_Feedbacks_BehaviorRating",
                table: "Feedbacks",
                sql: "[BehaviorRating] >= 1 AND [BehaviorRating] <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Feedbacks_DurabilityRating",
                table: "Feedbacks",
                sql: "[DurabilityRating] >= 1 AND [DurabilityRating] <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Feedbacks_HygieneRating",
                table: "Feedbacks",
                sql: "[HygieneRating] >= 1 AND [HygieneRating] <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Feedbacks_QualityRating",
                table: "Feedbacks",
                sql: "[QualityRating] >= 1 AND [QualityRating] <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Feedbacks_BehaviorRating",
                table: "Feedbacks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Feedbacks_DurabilityRating",
                table: "Feedbacks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Feedbacks_HygieneRating",
                table: "Feedbacks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Feedbacks_QualityRating",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "BehaviorRating",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DurabilityRating",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "HygieneRating",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "QualityRating",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Feedbacks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }
    }
}
