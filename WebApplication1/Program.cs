using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// Configurar el serializador JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	options.JsonSerializerOptions.MaxDepth = 64; 
});

// Agrega soporte para controladores con vistas (MVC).
builder.Services.AddControllersWithViews();

// Agrega soporte para controladores API.
builder.Services.AddControllers();

// Agrega un cliente HTTP para hacer solicitudes HTTP.
builder.Services.AddHttpClient();

// Agrega el servicio IApiService.
builder.Services.AddScoped<IApiService, ApiService>();

// Configura el contexto de la base de datos con SQL Server usando la cadena de conexión del archivo de configuración.
builder.Services.AddDbContext<DbptContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Agrega el servicio de descarga de usuarios
builder.Services.AddScoped<UserDownloadService>();

// Agrega el servicio de fondo para ejecutar tareas en segundo plano.
builder.Services.AddHostedService<BackgroundService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");


app.Run();
