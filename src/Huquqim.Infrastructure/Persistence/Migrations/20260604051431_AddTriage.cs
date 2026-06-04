using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huquqim.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTriage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "recommend_lawyer",
                table: "cases",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "triage_json",
                table: "cases",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recommend_lawyer",
                table: "cases");

            migrationBuilder.DropColumn(
                name: "triage_json",
                table: "cases");
        }
    }
}
