using EskitechAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();