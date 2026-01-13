var builder = WebApplication.CreateBuilder(args);

// ? Configurar puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
{
   Title = "Microservicio de Facturas - Café San Juan",
    Version = "v1",
        Description = "API para gestión de facturas y pagos"
    });
});

// ? Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
  policy.AllowAnyOrigin()
     .AllowAnyMethod()
      .AllowAnyHeader();
    });
});

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// ? Habilitar CORS
app.UseCors("AllowAll");

// Middleware para capturar excepciones globales
app.Use(async (context, next) =>
{
    try
 {
     await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
   logger.LogError(ex, "Error no controlado en la aplicación");
  
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new 
        { 
            error = "Error interno del servidor", 
    message = ex.Message,
            stackTrace = ex.StackTrace 
        });
    }
});

// Configure the HTTP request pipeline - Swagger SIEMPRE habilitado
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Facturas API v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
});

app.UseAuthorization();
app.MapControllers();

app.Run();
