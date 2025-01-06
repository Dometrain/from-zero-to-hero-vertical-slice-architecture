using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelInspiration.API.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SuggestedFieldAddedToStop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Suggested",
                table: "Stops",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suggested",
                table: "Stops");
        }
    }
}
