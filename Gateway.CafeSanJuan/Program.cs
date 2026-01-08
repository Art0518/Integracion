using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configurar Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// Agregar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
    policy.AllowAnyOrigin()
              .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Habilitar CORS
app.UseCors("AllowAll");

// Usar Ocelot como API Gateway
await app.UseOcelot();

app.Run();
