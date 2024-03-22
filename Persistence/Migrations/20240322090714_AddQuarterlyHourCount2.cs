using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PipelineApp2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddQuarterlyHourCount2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Hours",
                table: "QuarterlyHours",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Hours",
                table: "QuarterlyHours",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
