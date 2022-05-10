var notificacaoConnection = new signalR.HubConnectionBuilder().withUrl("/notificacaoHub").build();

notificacaoConnection.on("Notificacao", (origem, mensagem) => {
    exibeNotificacao(origem, mensagem);
});

notificacaoConnection.start().then(() => {
    exibeNotificacao("javascript", "Conectado ao Web Socket.");
    document.getElementById("teste-notificacao")?.addEventListener("click", () => {
        notificacaoConnection.invoke("Notificar", "frontend", "Teste");
    });
}).catch(function (err) {
    exibeNotificacao("javascript", "Erro ao conectar no Web Socket.");
    return console.error(err.toString());
});
