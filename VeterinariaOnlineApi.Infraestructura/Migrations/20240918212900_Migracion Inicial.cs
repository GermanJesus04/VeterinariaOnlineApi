using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VeterinariaOnlineApi.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FECHA_CREACION = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FECHA_ACTUALIZACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FECHA_ELIMINACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REGISTRO_ELIMINADO = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "REFRESH_TOKEN",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    USER_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JWT_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TICKET_TOKEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TOKEN_USADO = table.Column<bool>(type: "bit", nullable: false),
                    TOKEN_REVOCADO = table.Column<bool>(type: "bit", nullable: false),
                    FECHA_AGREGADO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FECHA_CADUCIDAD = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REFRESH_TOKEN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MASCOTA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NOMBRE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ESPECIE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EDAD = table.Column<int>(type: "int", nullable: false),
                    DueñoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FECHA_CREACION = table.Column<DateTime>(type: "datetime2", nullable: false),
                    USUARIO_CREACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FECHA_ACTUALIZACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USUARIO_ACTUALIZACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    REGISTRO_ELIMINADO = table.Column<bool>(type: "bit", nullable: false),
                    FECHA_ELIMINACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USUARIO_ELIMINACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MASCOTA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MASCOTA_AspNetUsers_DueñoId",
                        column: x => x.DueñoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CITA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MascotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FECHA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FECHA_CREACION = table.Column<DateTime>(type: "datetime2", nullable: false),
                    USUARIO_CREACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FECHA_ACTUALIZACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USUARIO_ACTUALIZACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    REGISTRO_ELIMINADO = table.Column<bool>(type: "bit", nullable: false),
                    FECHA_ELIMINACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USUARIO_ELIMINACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CITA_MASCOTA_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "MASCOTA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRATAMIENTO",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NOMBRE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MascotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FECHA_CREACION = table.Column<DateTime>(type: "datetime2", nullable: false),
                    USUARIO_CREACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FECHA_ACTUALIZACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USUARIO_ACTUALIZACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    REGISTRO_ELIMINADO = table.Column<bool>(type: "bit", nullable: false),
                    FECHA_ELIMINACION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USUARIO_ELIMINACION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRATAMIENTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_MASCOTA_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "MASCOTA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d98504fb-474b-45ee-be91-637dd885d715", null, "Dueño", "DUEÑO" },
                    { "ea71a7cd-ee89-41d3-a8bb-d1c425a9bf8a", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CITA_MascotaId",
                table: "CITA",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_MASCOTA_DueñoId",
                table: "MASCOTA",
                column: "DueñoId");

            migrationBuilder.CreateIndex(
                name: "IX_MASCOTA_NOMBRE_ESPECIE_EDAD",
                table: "MASCOTA",
                columns: new[] { "NOMBRE", "ESPECIE", "EDAD" });

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_MascotaId",
                table: "TRATAMIENTO",
                column: "MascotaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CITA");

            migrationBuilder.DropTable(
                name: "REFRESH_TOKEN");

            migrationBuilder.DropTable(
                name: "TRATAMIENTO");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MASCOTA");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
