using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GastroApp.Migrations
{
    /// <inheritdoc />
    public partial class TestRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 17, 34, 24, 830, DateTimeKind.Local).AddTicks(7112),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 567, DateTimeKind.Local).AddTicks(7980));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "OrderedMeals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 17, 34, 24, 832, DateTimeKind.Local).AddTicks(3156),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 570, DateTimeKind.Local).AddTicks(4592));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 567, DateTimeKind.Local).AddTicks(7980),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 17, 34, 24, 830, DateTimeKind.Local).AddTicks(7112));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "OrderedMeals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 570, DateTimeKind.Local).AddTicks(4592),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 17, 34, 24, 832, DateTimeKind.Local).AddTicks(3156));
        }
    }
}
