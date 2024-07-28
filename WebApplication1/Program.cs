using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Service;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configurar el serializador JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	options.JsonSerializerOptions.MaxDepth = 64; // Puedes ajustar la profundidad máxima si es necesario
});

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiService, ApiService>();

builder.Services.AddDbContext<DbptContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Servicio de descarga
builder.Services.AddScoped<UserDownloadService>();
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
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Users}/{action=Index}/{id?}");


app.Run();
