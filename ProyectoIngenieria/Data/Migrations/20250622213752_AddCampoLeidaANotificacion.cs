using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoIngenieria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCampoLeidaANotificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Leida",
                table: "NOTIFICACION",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leida",
                table: "NOTIFICACION");
        }
    }
}
