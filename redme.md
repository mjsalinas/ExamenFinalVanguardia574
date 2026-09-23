4.2 Llamarlo desde EventosController.cs

Agregá el using arriba:

csharp
using TicketExpress.Validation;

En Create, después de las dos líneas de normalización y antes del _db.Eventos.Add(evento);:

csharp
        var error = EventoValidator.Validar(evento);
        if (error is not null)
            return BadRequest(error);

En Update, después de asignar todos los valores nuevos y antes del await _db.SaveChangesAsync();:

csharp
        var error = EventoValidator.Validar(evento);
        if (error is not null)
            return BadRequest(error);