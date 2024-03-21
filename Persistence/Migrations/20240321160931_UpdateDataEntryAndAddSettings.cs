using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PipelineApp2._0.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataEntryAndAddSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DateEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "DateEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkingDaysPerWeek = table.Column<string>(type: "TEXT", nullable: false),
                    WorkingHoursPerDay = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DateEntries");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "DateEntries");
        }
    }
}
