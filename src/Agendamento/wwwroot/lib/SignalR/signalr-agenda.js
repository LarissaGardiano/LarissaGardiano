const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("IncluirAgendamentoAtendente", (dados) => {
    if ($('.btnCarregarAgenda').hasClass('arredondar-lados'))
        return;

    var dataInicio = new Date(dados.calendario.inicio);
    var formattedDataInicio = dataInicio.toISOString().split('T')[0];

    if ($('.dataCalendario').val() !== formattedDataInicio)
        return;

    if (AtendenteLogado.IdAtendente != dados.idAtendente)
        return;

    $('#calendar').fullCalendar('renderEvent', {
        id: dados.calendario.idAgendamento,
        title: dados.calendario.nomeServico,
        start: new Date(...dados.calendario.inicioExibir.split(",")),
        end: new Date(...dados.calendario.terminoExibir.split(",")),
        allDay: false,
        className: 'success',
        nomeCliente: dados.calendario.nomeCliente
    }, true);
});

connection.on("ConectadoAoHub", (mensagem) => {
    console.log(mensagem);
});

connection.on("ClienteDesconectado", (connectionId) => {
    console.log(`Cliente desconectado: ${connectionId}`);
});

connection.on("RemoverAgendamentoAtendente", (idAgendamento) => {
    if ($('.btnCarregarAgenda').hasClass('arredondar-lados'))
        return;

    $('#calendar').fullCalendar('removeEvents', idAgendamento);
});

connection.start().catch(err => 
{
    alert("Erro ao conectar no SignalR:\n" + err.toString());
    console.error("Detalhes completos do erro SignalR:", err);
});