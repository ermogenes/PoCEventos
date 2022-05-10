using ksqlDB.RestApi.Client.KSql.Query.Context;
using ksqlDB.RestApi.Client.KSql.Linq;
using ksqlDB.RestApi.Client.KSql.Query.Options;

using Microsoft.AspNetCore.SignalR.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Iniciando...");

// Obtém configuração
KafkaInfra ambiente = new();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();

        IHostEnvironment env = hostingContext.HostingEnvironment;

        Console.WriteLine($"ambiente = {env.EnvironmentName}");

        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        IConfigurationRoot configurationRoot = configuration.Build();

        configurationRoot.GetSection(nameof(KafkaInfra)).Bind(ambiente);
    }).Build();

Console.WriteLine($"ksqldb  = {ambiente.ksqlDbUrl}");
Console.WriteLine($"signalr = {ambiente.SignalRHubUrl}");

// Conexão com KsqlDB
var options = new KSqlDBContextOptions(ambiente.ksqlDbUrl!)
{
    ShouldPluralizeFromItemName = false
};

await using var _ksql = new KSqlDBContext(options);

// Conexão com SignalR Hub
var _hub = new HubConnectionBuilder()
    .WithUrl(ambiente.SignalRHubUrl!)
    .WithAutomaticReconnect()
    .Build();

await _hub.StartAsync();

// Subscribe no stream do KsqlDB
using var disposable = _ksql.CreateQueryStream<Notificacao>(fromItemName: "notificacoes")
    .WithOffsetResetPolicy(AutoOffsetReset.Latest)
    .Subscribe(async (n) =>
    {
        Console.WriteLine($"Notificar {n.origem} => {n.mensagem}");
        
        // Publica no SignalR
        await _hub.InvokeAsync("Notificar", n.origem, n.mensagem);
    },
        error => { Console.WriteLine($"Exception: {error.Message}"); },
        () => Console.WriteLine("Finalizado.")
    );

Console.WriteLine("OK ==> Aguardando notificações a enviar (CTRL+C para finalizar).");

while(true){};

public class Notificacao
{
    public string? origem { get; set; }
    public string? mensagem { get; set; }
};

public class KafkaInfra
{
    public string? ksqlDbUrl { get; set; }
    public string? SignalRHubUrl { get; set; }
};
