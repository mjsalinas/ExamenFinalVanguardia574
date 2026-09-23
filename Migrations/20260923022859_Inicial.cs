using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketExpress.Migrations;

/// <inheritdoc />
public partial class _20260923022859_Inicial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Eventos",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Nombre = table.Column<string>(type: "TEXT", nullable: false),
                Ciudad = table.Column<string>(type: "TEXT", nullable: false),
                Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                CapacidadTotal = table.Column<int>(type: "INTEGER", nullable: false),
                PrecioBoleto = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Eventos", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Boletos",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                NombreComprador = table.Column<string>(type: "TEXT", nullable: false),
                CorreoComprador = table.Column<string>(type: "TEXT", nullable: false),
                Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                FechaCompra = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Boletos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Boletos_Eventos_EventoId",
                    column: x => x.EventoId,
                    principalTable: "Eventos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Boletos_EventoId",
            table: "Boletos",
            column: "EventoId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Boletos");

        migrationBuilder.DropTable(
            name: "Eventos");
    }
}
