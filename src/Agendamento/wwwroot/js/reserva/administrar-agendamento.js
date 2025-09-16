
function btnAbrirModalAgendamentoServico_OnClick() {
    var dataAgendamento = new Date($('.card-dia-selecionado').eq(0).attr('data-ano'), $('.card-dia-selecionado').eq(0).attr('data-mes'), $('.card-dia-selecionado').eq(0).attr('data-dia'));
    var horaAgendamento = $(this).text();

    const formattedDate = dataAgendamento.toLocaleDateString('pt-BR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });

    var nomeAtendente = $('.card-atendente-selecionado .card-title').eq(0)[0].innerText;

    $('#atendenteSelecionado').html('&nbsp;' + nomeAtendente);
    $('#dataSelecionado').html('&nbsp;' + formattedDate + ' às ' + horaAgendamento + 'h');

    $('#btnIncluirAgendamento').attr('data-horas', horaAgendamento)
    $('#mensagem-alerta').text('');
    $('#modal-incluir-agendamento').modal('show');
}

function btnIncluirAgendamento_OnClick() {
    $('#mensagem-alerta').text('');
    var dataAgendamento = new Date($('.card-dia-selecionado').eq(0).attr('data-ano'), $('.card-dia-selecionado').eq(0).attr('data-mes'), $('.card-dia-selecionado').eq(0).attr('data-dia'));
    const [hours, minutes] = $(this).attr('data-horas').split(':');

    dataAgendamento.setUTCHours(parseInt(hours));
    dataAgendamento.setUTCMinutes(parseInt(minutes));

    var dados = {
        idServico: Servico.IdServico,
        idAtendente: $('.card-atendente-selecionado').eq(0).attr('data-id'),
        nomeCliente: $('#meuNome').val(),
        telefoneCliente: $('#meuTelefone').val(),
        dataAgendamento: dataAgendamento.toISOString()
    };

    apiPost(dados, '/Agendamento/Incluir').done(function (retorno) {
        if (retorno.sucesso) {
            $('#modal-incluir-agendamento').modal('hide');
            $('#meuTelefoneReservas').val(dados.telefoneCliente);
            
            $('#modal-sucesso-agendamento').modal('show');
            BuscarHorariosDisponiveisAgenda();
        }
        else {
            $('#mensagem-alerta').text(retorno.mensagens[0]);
        }
    });
}

function documentReadyAdministrarAgendamento() {
    $("body").delegate('#btnAbrirModalAgendamentoServico', 'click', btnAbrirModalAgendamentoServico_OnClick);
    $("body").delegate('#btnIncluirAgendamento', 'click', btnIncluirAgendamento_OnClick);
}

$(document).ready(documentReadyAdministrarAgendamento);