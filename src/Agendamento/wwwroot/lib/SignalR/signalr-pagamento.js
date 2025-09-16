const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ConectadoAoHub", (mensagem) => {
    console.log(mensagem);
});

connection.on("ClienteDesconectado", (connectionId) => {
    console.log(`Cliente desconectado: ${connectionId}`);
});

connection.on("AtualizarPagamentoPagbank", (mensagem, idSalao) => {
    if (IdSalao != idSalao)
        return;

    if ($("#modal-credito-pagamento").is(":visible")) {
        $('#mensagem-alerta-incluir-pagamento-credito').text(mensagem);
        $('#btnPagarNoCredito').text('Pagar').prop('disabled', false);
    }

    if ($("#modal-pix-pagamento").is(":visible")) {
        $('#mensagem-alerta-incluir-pagamento-pix').text(mensagem);
        $('#btnCopiarPix').text('Copiar');
    }
});

connection.start().catch(err => {
    alert("Erro ao conectar no SignalR:\n" + err.toString());
    console.error("Detalhes completos do erro SignalR:", err);
});