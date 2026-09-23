using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress.Common;

public static class DisponibilidadCalculator
{
    public static async Task<int> BoletosDisponiblesAsync(TicketExpressDbContext db, Evento evento)
    {
        var vendidos = await db.Boletos
            .Where(b => b.EventoId == evento.Id)
            .SumAsync(b => (int?)b.Cantidad) ?? 0;

        return evento.CapacidadTotal - vendidos;
    }
}