using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Web.db;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<lojaContext>(opt =>
{
    string connectionString = builder.Configuration.GetConnectionString("lojaConnectionString");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);
});

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/produtos", ([FromServices] lojaContext _db) =>
{
    return Results.Ok(_db.Produto.ToList<Produto>());
});

app.MapGet("/api/vendas", ([FromServices] lojaContext _db) =>
{
    return Results.Ok(_db.Venda.ToList<Venda>());
});

app.MapPost("/api/pedidos", (
    [FromServices] lojaContext _db,
    [FromBody] Pedido pedido
) =>
{
    // Adiciona no tópico "pedido"

    return Results.Created($"/api/vendas", pedido);
});

app.Run();


public record Pedido(string produtoId, int quantidade);
