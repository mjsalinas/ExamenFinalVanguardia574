using TicketExpress;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=biblioteca.db"
    ));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
//buiders para activar interaz de swagger y openapi, para poder probar la api desde el navegador
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.Migrate();
}
//swager y openapi solo se activan en desarrollo, para no exponer la documentación de la api en producción
if (app.Environment.IsDevelopment())
{
    //swagger y openapi solo se activan en desarrollo, para no exponer la documentación de la api en producción
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();