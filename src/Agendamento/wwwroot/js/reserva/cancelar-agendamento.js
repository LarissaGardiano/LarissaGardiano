function btnAbrirModalCancelarAgendamento_OnClick() {
    var idAgendamento = $(this).attr('data-id');
    var detalheAgendamento = $('#detalhes-agendamento-' + idAgendamento)[0].innerHTML;

    $('#VerDetalheAgendamento').html(detalheAgendamento);
    $('#btnConfirmarCancelamentoAgendamento').attr('data-id', idAgendamento);
    $('#modal-cancelar-agendamento').modal('show');
}

function btnAbrirModalAvisoCancelamento_OnClick() {
    $('#modal-aviso-cancelar-agendamento').modal('show');
}

function btnConfirmarCancelamentoAgendamento_OnClick() {
    var idAgendamento = $(this).attr('data-id');

    apiGet("/cancelar-agendamento/" + idAgendamento).done(function (retorno) {
        window.location.reload();
    });
}

function documentReadyCancelarAgendamento() {
    $("body").delegate('#btnAbrirModalCancelarAgendamento', 'click', btnAbrirModalCancelarAgendamento_OnClick);
    $("body").delegate('#btnAbrirModalAvisoCancelamento', 'click', btnAbrirModalAvisoCancelamento_OnClick);
    $("body").delegate('#btnConfirmarCancelamentoAgendamento', 'click', btnConfirmarCancelamentoAgendamento_OnClick);
}

$(document).ready(documentReadyCancelarAgendamento);