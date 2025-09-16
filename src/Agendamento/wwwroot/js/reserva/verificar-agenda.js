
function SelecionarHojeDefault() {
    var hoje = new Date();
    var diaSemanaAtual = DiasSemana[hoje.getDay()];

    $('.dia-semana').each(function () {
        var diaSemanaElemento = $(this).find('div').text();

        if (diaSemanaElemento === diaSemanaAtual) {
            $(this).addClass('card-dia-selecionado');
        }
    });

    BuscarHorariosDisponiveisAgenda();
}

function AlterarCardAtendenteSelecionado_OnClick() {
    $('.card-atendente').removeClass('card-atendente-selecionado');
    $(this).addClass('card-atendente-selecionado');

    BuscarHorariosDisponiveisAgenda();
}

function AlterarCardAtendenteDiaSelecionado_OnClick() {
    $('.dia-semana').removeClass('card-dia-selecionado');
    $(this).addClass('card-dia-selecionado');

    BuscarHorariosDisponiveisAgenda();
    $('#mes-ano-selecionado').text(obterMesAnoSelecionado(null));
}

function BuscarHorariosDisponiveisAgenda() {
    var html = '';
    $('#conteudo-horarios').html(html);

    var idAtendente = $('.card-atendente-selecionado').eq(0).attr('data-id');
    var dataSelecionada = new Date($('.card-dia-selecionado').eq(0).attr('data-ano'), $('.card-dia-selecionado').eq(0).attr('data-mes'), $('.card-dia-selecionado').eq(0).attr('data-dia'));

    if (isNaN(dataSelecionada.getTime()))
        return;

    const dataFormatada = new Date(dataSelecionada).toISOString().split('T')[0];
    const url = `/HorarioAgenda/BuscarHorariosDisponiveis?idAtendente=${idAtendente}&idServico=${Servico.IdServico}&data=${dataFormatada}`;

    apiGet(url).done(function (retorno) {
        if (retorno.length == 0) {
            $('#conteudo-horarios').html(`<label>Nenhum horário disponível</label>`);
            return;
        }

        for (var i = 0; i < retorno.length; i++) {
            html += `<label class="card-hora" id="btnAbrirModalAgendamentoServico">${retorno[i].substring(0, 5)}</label>`;
        }

        $('#conteudo-horarios').html(html);
    });
}

function obterMesAnoSelecionado(date) {
    var meses = [
        "JANEIRO", "FEVEREIRO", "MARÇO", "ABRIL", "MAIO", "JUNHO",
        "JULHO", "AGOSTO", "SETEMBRO", "OUTUBRO", "NOVEMBRO", "DEZEMBRO"
    ];

    var hoje = new Date();
    if (date != null)
        hoje = date;

    if ($('.dia-semana').hasClass('card-dia-selecionado'))
        hoje = new Date($('.card-dia-selecionado').attr('data-ano'), $('.card-dia-selecionado').attr('data-mes'), $('.card-dia-selecionado').attr('data-dia'));

    var mesAtual = meses[hoje.getMonth()];
    var anoAtual = hoje.getFullYear();

    return mesAtual + " " + anoAtual;
}

function documentReadyVerificarAgenda() {
    $("body").delegate('.card-atendente', 'click', AlterarCardAtendenteSelecionado_OnClick);
    $("body").delegate('.dia-semana', 'click', AlterarCardAtendenteDiaSelecionado_OnClick);

    $('.card-atendente').eq(0).addClass('card-atendente-selecionado');
    $('#mes-ano-selecionado').text(obterMesAnoSelecionado(null));
    SelecionarHojeDefault();
}

$(document).ready(documentReadyVerificarAgenda);