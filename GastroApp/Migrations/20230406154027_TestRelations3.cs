using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GastroApp.Migrations
{
    /// <inheritdoc />
    public partial class TestRelations3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 15, 40, 26, 976, DateTimeKind.Utc).AddTicks(173),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 15, 37, 18, 361, DateTimeKind.Utc).AddTicks(6130));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "OrderedMeals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 15, 40, 26, 977, DateTimeKind.Utc).AddTicks(3601),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 17, 37, 18, 362, DateTimeKind.Local).AddTicks(7700));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 15, 37, 18, 361, DateTimeKind.Utc).AddTicks(6130),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 15, 40, 26, 976, DateTimeKind.Utc).AddTicks(173));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "OrderedMeals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 17, 37, 18, 362, DateTimeKind.Local).AddTicks(7700),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 15, 40, 26, 977, DateTimeKind.Utc).AddTicks(3601));
        }
    }
}
