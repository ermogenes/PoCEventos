using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

using Confluent.Kafka;
using StackExchange.Redis;

using Web.db;
using Web.kafka;
using Web.Hubs;

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

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt =>
{
  opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

string kafkaServer = builder.Configuration
    .GetSection("Dependencies:Broker:Producer").GetValue<string>("BootstrapServers");

string pedidoTopic = builder.Configuration
    .GetSection("Dependencies:Broker").GetValue<string>("Topic");

string redisServer = builder.Configuration
    .GetSection("Dependencies:Redis").GetValue<string>("Url");

builder.Services.AddSingleton<KafkaClientHandle>();
builder.Services.AddSingleton<KafkaDependentProducer<long, string>>();
builder.Services.AddSingleton<DeliveryHandler>();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisServer)
);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<NotificacaoHub>("/notificacaoHub");

app.MapGet("/api/produtos", ([FromServices] lojaContext _db) =>
{
    return Results.Ok(_db.Produto.ToList<Produto>());
});

app.MapGet("/api/vendas", ([FromServices] lojaContext _db) =>
{
    return Results.Ok(_db.Venda
        .Include(v => v.Produto)
        .OrderByDescending(v => v.Id)
        .Take(20)
        .ToList<Venda>()
    );
});

app.MapPost("/api/pedidos", async (
    [FromServices] IConnectionMultiplexer _connmux,
    [FromServices] KafkaDependentProducer<long, string> _producer,
    [FromServices] DeliveryHandler _delivery,
    [FromBody] Pedido pedido
) =>
{
    var redis = _connmux.GetDatabase();

    long id = await redis.StringIncrementAsync("pedidosQtd");
    string content = JsonSerializer.Serialize(pedido);

    _producer.Produce(
        pedidoTopic,
        new Message<long, string> { Key = id, Value = content },
        _delivery.DeliveryReportHandler
    );

    return Results.Created($"/api/vendas", pedido);
});

app.Run();


public record Pedido(string produtoId, int quantidade);
