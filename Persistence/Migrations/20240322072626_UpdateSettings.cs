using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PipelineApp2._0.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDaysPerWeek",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "WorkingHoursPerDay",
                table: "Settings",
                newName: "LunchBreakInMinutes");

            migrationBuilder.AddColumn<bool>(
                name: "AddLunchBreaks",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Colour = table.Column<string>(type: "TEXT", nullable: false),
                    SettingId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Settings_SettingId",
                        column: x => x.SettingId,
                        principalTable: "Settings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WeekDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsWorkDay = table.Column<bool>(type: "INTEGER", nullable: false),
                    Hours = table.Column<int>(type: "INTEGER", nullable: false),
                    Minutes = table.Column<int>(type: "INTEGER", nullable: false),
                    SettingId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeekDays_Settings_SettingId",
                        column: x => x.SettingId,
                        principalTable: "Settings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_SettingId",
                table: "Tags",
                column: "SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekDays_SettingId",
                table: "WeekDays",
                column: "SettingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "WeekDays");

            migrationBuilder.DropColumn(
                name: "AddLunchBreaks",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "LunchBreakInMinutes",
                table: "Settings",
                newName: "WorkingHoursPerDay");

            migrationBuilder.AddColumn<string>(
                name: "WorkingDaysPerWeek",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
