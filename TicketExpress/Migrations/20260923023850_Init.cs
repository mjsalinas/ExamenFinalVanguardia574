using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketExpress.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Ciudad = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CapacidadTotal = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioBoleto = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boleto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoIdId = table.Column<int>(type: "INTEGER", nullable: true),
                    NombreComprador = table.Column<string>(type: "TEXT", nullable: false),
                    CorreoComprador = table.Column<string>(type: "TEXT", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaCompra = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boleto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boleto_Evento_EventoIdId",
                        column: x => x.EventoIdId,
                        principalTable: "Evento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boleto_EventoIdId",
                table: "Boleto",
                column: "EventoIdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boleto");

            migrationBuilder.DropTable(
                name: "Evento");
        }
    }
}
