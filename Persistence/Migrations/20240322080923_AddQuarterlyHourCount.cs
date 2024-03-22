using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PipelineApp2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddQuarterlyHourCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuarterlyHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hours = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyHours", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuarterlyHours");
        }
    }
}
