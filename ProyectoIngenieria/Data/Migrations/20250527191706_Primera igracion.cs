using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoIngenieria.Data.Migrations
{
    /// <inheritdoc />
    public partial class Primeraigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CATALOGO_MANTENIMIENTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CATALOGO_MANTENIMIENTO_pk", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPRESA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EMPRESA_pk", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VEHICULO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Placa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EMPRESA_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VEHICULO_pk", x => x.ID);
                    table.ForeignKey(
                        name: "MAQUINA_EMPRESA",
                        column: x => x.EMPRESA_ID,
                        principalTable: "EMPRESA",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENTO_VEHICULO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ruta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VEHICULO_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DOCUMENTO_VEHICULO_pk", x => x.ID);
                    table.ForeignKey(
                        name: "DOCUMENTO_MAQUINA",
                        column: x => x.VEHICULO_ID,
                        principalTable: "VEHICULO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HORAS_TRABAJO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Horometro_Inicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Horometro_Final = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lugar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Precio_Hora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VEHICULO_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("HORAS_TRABAJO_pk", x => x.ID);
                    table.ForeignKey(
                        name: "HORAS_TRABAJO_MAQUINA",
                        column: x => x.VEHICULO_ID,
                        principalTable: "VEHICULO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "NOTIFICACION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    VEHICULO_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("NOTIFICACION_pk", x => x.ID);
                    table.ForeignKey(
                        name: "NOTIFICACION_MAQUINA",
                        column: x => x.VEHICULO_ID,
                        principalTable: "VEHICULO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OPERADOR",
                columns: table => new
                {
                    Cedula = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VEHICULO_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("OPERADOR_pk", x => x.Cedula);
                    table.ForeignKey(
                        name: "OPERADOR_MAQUINA",
                        column: x => x.VEHICULO_ID,
                        principalTable: "VEHICULO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "REGISTRO_COMBUSTIBLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha_Compra = table.Column<DateOnly>(type: "date", nullable: false),
                    Litros_Comprados = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio_Litro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Pagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VEHICULO_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("REGISTRO_COMBUSTIBLE_pk", x => x.ID);
                    table.ForeignKey(
                        name: "REGISTRO_COMBUSTIBLE_MAQUINA",
                        column: x => x.VEHICULO_ID,
                        principalTable: "VEHICULO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "REGISTRO_MANTENIMIENTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    VEHICULO_ID = table.Column<int>(type: "int", nullable: false),
                    CATALOGO_MANTENIMIENTO_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("REGISTRO_MANTENIMIENTO_pk", x => x.ID);
                    table.ForeignKey(
                        name: "REGISTRO_MANTENIMIENTO_CATALOGO_MANTENIMIENTO",
                        column: x => x.CATALOGO_MANTENIMIENTO_ID,
                        principalTable: "CATALOGO_MANTENIMIENTO",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "REGISTRO_MANTENIMIENTO_MAQUINA",
                        column: x => x.VEHICULO_ID,
                        principalTable: "VEHICULO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENTO_OPERADOR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ruta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OPERADOR_Cedula = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DOCUMENTO_OPERADOR_pk", x => x.ID);
                    table.ForeignKey(
                        name: "DOCUMENTO_OPERADOR_OPERADOR",
                        column: x => x.OPERADOR_Cedula,
                        principalTable: "OPERADOR",
                        principalColumn: "Cedula");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTO_OPERADOR_OPERADOR_Cedula",
                table: "DOCUMENTO_OPERADOR",
                column: "OPERADOR_Cedula");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTO_VEHICULO_VEHICULO_ID",
                table: "DOCUMENTO_VEHICULO",
                column: "VEHICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HORAS_TRABAJO_VEHICULO_ID",
                table: "HORAS_TRABAJO",
                column: "VEHICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_NOTIFICACION_VEHICULO_ID",
                table: "NOTIFICACION",
                column: "VEHICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OPERADOR_VEHICULO_ID",
                table: "OPERADOR",
                column: "VEHICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REGISTRO_COMBUSTIBLE_VEHICULO_ID",
                table: "REGISTRO_COMBUSTIBLE",
                column: "VEHICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REGISTRO_MANTENIMIENTO_CATALOGO_MANTENIMIENTO_ID",
                table: "REGISTRO_MANTENIMIENTO",
                column: "CATALOGO_MANTENIMIENTO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REGISTRO_MANTENIMIENTO_VEHICULO_ID",
                table: "REGISTRO_MANTENIMIENTO",
                column: "VEHICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_VEHICULO_EMPRESA_ID",
                table: "VEHICULO",
                column: "EMPRESA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOCUMENTO_OPERADOR");

            migrationBuilder.DropTable(
                name: "DOCUMENTO_VEHICULO");

            migrationBuilder.DropTable(
                name: "HORAS_TRABAJO");

            migrationBuilder.DropTable(
                name: "NOTIFICACION");

            migrationBuilder.DropTable(
                name: "REGISTRO_COMBUSTIBLE");

            migrationBuilder.DropTable(
                name: "REGISTRO_MANTENIMIENTO");

            migrationBuilder.DropTable(
                name: "OPERADOR");

            migrationBuilder.DropTable(
                name: "CATALOGO_MANTENIMIENTO");

            migrationBuilder.DropTable(
                name: "VEHICULO");

            migrationBuilder.DropTable(
                name: "EMPRESA");
        }
    }
}
