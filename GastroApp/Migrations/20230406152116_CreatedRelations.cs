using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GastroApp.Migrations
{
    /// <inheritdoc />
    public partial class CreatedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 567, DateTimeKind.Local).AddTicks(7980),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 15, 16, 11, 612, DateTimeKind.Local).AddTicks(4746));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "OrderedMeals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 570, DateTimeKind.Local).AddTicks(4592),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 15, 16, 11, 612, DateTimeKind.Local).AddTicks(6869));

            migrationBuilder.CreateIndex(
                name: "IX_Tables_RoomId",
                table: "Tables",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TableId",
                table: "Orders",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMeals_MealId",
                table: "OrderedMeals",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_CategoryId",
                table: "Meals",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Categories_CategoryId",
                table: "Meals",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedMeals_Meals_MealId",
                table: "OrderedMeals",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedMeals_Orders_OrderId",
                table: "OrderedMeals",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tables_TableId",
                table: "Orders",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Rooms_RoomId",
                table: "Tables",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Categories_CategoryId",
                table: "Meals");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedMeals_Meals_MealId",
                table: "OrderedMeals");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedMeals_Orders_OrderId",
                table: "OrderedMeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tables_TableId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Rooms_RoomId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_RoomId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TableId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderedMeals_MealId",
                table: "OrderedMeals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_CategoryId",
                table: "Meals");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 15, 16, 11, 612, DateTimeKind.Local).AddTicks(4746),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 567, DateTimeKind.Local).AddTicks(7980));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "OrderedMeals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 6, 15, 16, 11, 612, DateTimeKind.Local).AddTicks(6869),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 4, 6, 17, 21, 16, 570, DateTimeKind.Local).AddTicks(4592));
        }
    }
}
