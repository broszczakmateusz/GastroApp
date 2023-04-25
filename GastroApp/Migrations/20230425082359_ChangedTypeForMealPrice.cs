using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GastroApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTypeForMealPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "VATRate",
                table: "Meals",
                type: "real",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Meals",
                type: "real",
                precision: 7,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 7,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "VATRate",
                table: "Meals",
                type: "double precision",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Meals",
                type: "double precision",
                precision: 7,
                scale: 2,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldPrecision: 7,
                oldScale: 2);
        }
    }
}
