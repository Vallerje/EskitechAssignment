using EskitechAPI.Data;
using EskitechAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<PriceService>();
builder.Services.AddScoped<ExcelService>();

// Registrera ImportService för DI-container
builder.Services.AddScoped<ExcelService>();

/*builder.Services.AddSingleton<IConfiguration>(builder.Configuration);*/

// Lägg till SQLite-konfiguration
builder.Services.AddDbContext<EskitechContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EskitechDatabase")));

// Lägg till tjänster för controllers
builder.Services.AddControllers();

// Lägg till Swagger-konfiguration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfigurera HTTP-request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

/*app.UseMiddleware<ApiKeyMiddleware>();*/

app.Run();