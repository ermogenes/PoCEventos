using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using ksqlDB.RestApi.Client.KSql.RestApi.Http;
using ksqlDB.RestApi.Client.KSql.RestApi;

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

var ksqlDbUrl = builder.Configuration.GetSection("KafkaInfra").GetValue<string>("ksqlDdUrl");
builder.Services.AddScoped<KSqlDbRestApiClient>(s =>
    new KSqlDbRestApiClient(new HttpClientFactory(new Uri(ksqlDbUrl)))
);

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
    [FromServices] KSqlDbRestApiClient _ksql,
    [FromBody] Pedido pedido
) =>
{
    _ksql.InsertIntoAsync<Pedido>(pedido);
    return Results.Created($"/api/vendas", pedido);
});

app.Run();


public record Pedido(string produtoId, int quantidade);
