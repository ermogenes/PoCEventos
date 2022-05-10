using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class NotificacaoHub : Hub
    {
        public async Task Notificar(string origem, string mensagem)
        {
            await Clients.All.SendAsync("Notificacao", origem, mensagem);
        }
    }
}