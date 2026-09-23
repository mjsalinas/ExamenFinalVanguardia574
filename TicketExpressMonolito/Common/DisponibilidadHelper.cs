using Microsoft.EntityFrameworkCore;
using TicketExpressMonolito.Data;

namespace TicketExpressMonolito.Common;

public static class DisponibilidadHelper
{
    public static async Task<int> BoletosDisponiblesAsync(AppDbContext context, int eventoId)
    {
        var evento = await context.Eventos.FindAsync(eventoId);
        if (evento == null) return -1;

        var vendidos = await context.Boletos
            .Where(b => b.EventoId == eventoId)
            .SumAsync(b => (int?)b.Cantidad) ?? 0;

        return evento.CapacidadTotal - vendidos;
    }
}