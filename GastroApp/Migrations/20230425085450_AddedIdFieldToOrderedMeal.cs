using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GastroApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdFieldToOrderedMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedMeals",
                table: "OrderedMeals");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderedMeals",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedMeals",
                table: "OrderedMeals",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMeals_OrderId",
                table: "OrderedMeals",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedMeals",
                table: "OrderedMeals");

            migrationBuilder.DropIndex(
                name: "IX_OrderedMeals_OrderId",
                table: "OrderedMeals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderedMeals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedMeals",
                table: "OrderedMeals",
                columns: new[] { "OrderId", "MealId" });
        }
    }
}
