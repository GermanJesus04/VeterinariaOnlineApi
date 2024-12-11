using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VeterinariaOnlineApi.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadoToCita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d98504fb-474b-45ee-be91-637dd885d715");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea71a7cd-ee89-41d3-a8bb-d1c425a9bf8a");

            migrationBuilder.AddColumn<int>(
                name: "ESTADO",
                table: "CITA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e1b74c3-80ec-44b5-aa42-276dfd779ccf", null, "Dueño", "DUEÑO" },
                    { "4e7c95a7-3812-480d-a506-6de18a07b0a5", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e1b74c3-80ec-44b5-aa42-276dfd779ccf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e7c95a7-3812-480d-a506-6de18a07b0a5");

            migrationBuilder.DropColumn(
                name: "ESTADO",
                table: "CITA");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d98504fb-474b-45ee-be91-637dd885d715", null, "Dueño", "DUEÑO" },
                    { "ea71a7cd-ee89-41d3-a8bb-d1c425a9bf8a", null, "Admin", "ADMIN" }
                });
        }
    }
}
