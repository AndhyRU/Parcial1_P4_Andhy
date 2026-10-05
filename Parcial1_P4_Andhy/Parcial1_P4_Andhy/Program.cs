using Scalar.AspNetCore;
using Parcial1_P4_Andhy.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<NumbersService>();


builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider
        .GetRequiredService<NumbersService>();

    await service.InitializeAsync();
}


app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

