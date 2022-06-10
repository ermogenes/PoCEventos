using Confluent.Kafka;

using Microsoft.AspNetCore.SignalR.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("Iniciando...");

// Obtém configuração
Dependencies deps = new();

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

        configurationRoot.GetSection(nameof(Dependencies)).Bind(deps);
    }).Build();

// Conexão com SignalR Hub
var _hub = new HubConnectionBuilder()
    .WithUrl(deps.SignalRHub!.Url!)
    .WithAutomaticReconnect()
    .Build();

await _hub.StartAsync();

// Conexão com Kafka
string brokers = deps.Broker!.Consumer!.BootstrapServers!;
string topic = deps.Broker!.Topic!;

Console.WriteLine($"brokers  = {brokers}");
Console.WriteLine($"topic = {topic}");

var config = new ConsumerConfig
{
    GroupId = "NotificacaoWorkers",
    BootstrapServers = brokers,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

// Inicia consumidor e se subscreve no tópico
using (var consumer = new ConsumerBuilder<Ignore, string>(config)
                            .Build())
{
    consumer.Subscribe(topic);

    try
    {
        Console.WriteLine("OK ==> Aguardando notificações a enviar (CTRL+C para finalizar).");

        while (true)
        {
            try
            {
                var evt = consumer.Consume();

                var notificacao = JsonSerializer.Deserialize<Notificacao>(evt.Message.Value);

                Console.WriteLine($"Notificação: [{notificacao!.origem!}] diz: [{notificacao!.mensagem!}]");

                // Publica no SignalR
                await _hub.InvokeAsync("Notificar", notificacao!.origem!, notificacao!.mensagem!);
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Erro ao consumir: {ex.Message}");
                throw;
            }
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine($"Consumidor finalizado.");
        throw;
    }
};

public class Notificacao
{
    public string? origem { get; set; }
    public string? mensagem { get; set; }
};

public class Dependencies
{
    public Broker? Broker { get; set; }
    public SignalRHub? SignalRHub { get; set; }
};

public class SignalRHub
{
    public string? Url { get; set; }
};

public class Broker
{
    public string? Topic { get; set; }
    public Consumer? Consumer { get; set; }
};

public class Consumer
{
    public string? BootstrapServers { get; set; }
};