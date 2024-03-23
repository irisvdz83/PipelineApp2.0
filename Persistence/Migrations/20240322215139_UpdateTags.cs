using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PipelineApp2._0.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Settings_SettingId",
                table: "Tags");

            migrationBuilder.AlterColumn<Guid>(
                name: "SettingId",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Tags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Settings_SettingId",
                table: "Tags",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Settings_SettingId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Tags");

            migrationBuilder.AlterColumn<Guid>(
                name: "SettingId",
                table: "Tags",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Settings_SettingId",
                table: "Tags",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");
        }
    }
}
