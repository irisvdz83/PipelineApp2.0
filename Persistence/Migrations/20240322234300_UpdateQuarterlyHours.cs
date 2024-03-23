using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PipelineApp2._0.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuarterlyHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Hours",
                table: "QuarterlyHours",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Hours",
                table: "QuarterlyHours",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT");
        }
    }
}
