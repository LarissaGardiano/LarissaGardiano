var DiasSemana = ['DOM', 'SEG', 'TER', 'QUA', 'QUI', 'SEX', 'SAB'];

function btnAbrirModalMeusAgendamentos_OnClick() {
    $('#modal-meus-agendamentos').modal('show');
}

function btnArmazenarTelefone_OnClick() {
    var telefone = $('#meuTelefoneReservas').val();

    if (telefone == '' || telefone == null || telefone.length < 14)
        return;

    $('#modal-meus-agendamentos').modal('hide');
    window.location.replace("/Agendamento/ArmazenarTelefone?telefone=" + encodeURIComponent(telefone));
}

function aplicarMascaras() {
    var behavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        options = {
            onKeyPress: function (val, e, field, options) {
                field.mask(behavior.apply({}, arguments), options);
            }
        };

    $('.mask-telefone').mask(behavior, options);
    $('.mask-real').mask("000.000,00", { reverse: true });
    $('.mask-cep').mask("00000-000");
    $('.mask-tempo').mask("00:00");
    $('.mask-prazo').mask("00/0000");
    $('.mask-cartao').mask("0000 0000 0000 0000");

    var optionsCpfCnpj = {
        onKeyPress: function (cpf, ev, el, op) {
            var masks = ['000.000.000-000', '00.000.000/0000-00'];
            $('.mask-cpf-cnpj').mask((cpf.length > 14) ? masks[1] : masks[0], op);
        }
    }

    $('.mask-cpf-cnpj').length > 11 ? $('.mask-cpf-cnpj').mask('00.000.000/0000-00', optionsCpfCnpj) : $('.mask-cpf-cnpj').mask('000.000.000-00#', optionsCpfCnpj);
}

window.addEventListener('error', function (event) {
    bloqueioDeTela(false);
});

function alterarVisualizacaoCard_OnClick() {
    alterarVisualizacaoExpandir(false);
    FecharTodosCardsAdministracao();

    var card = $(this).closest('.card');
    var cardBody = card.find('.card-body');
    var cardHeader = card.find('.card-header');
    var eye = card.find('.card-header i');
    $('.card-header i').removeClass('fa-eye-slash').addClass('fa-eye');

    if (cardBody.is(":visible")) {
        cardBody.slideUp(600);
        cardHeader.addClass('arredondar-lados');
        eye.removeClass('fa-eye-slash').addClass('fa-eye');
        alterarVisualizacaoExpandir(false);
    } else {
        cardBody.slideDown(600);
        cardHeader.removeClass('arredondar-lados');
        eye.removeClass('fa-eye').addClass('fa-eye-slash');
        alterarVisualizacaoExpandir(true);
    }
}

function alterarVisualizacaoExpandir(visualizar) {
    $('#MaximizarCalendario').addClass('hide');
    $('#btnAbrirModalTodasAgendas').addClass('hide');
    $('#btnAbrirModalFecharAgenda').addClass('hide');

    setTimeout(function () {
        if (visualizar) {
            $('#MaximizarCalendario').removeClass('hide');
            $('#btnAbrirModalTodasAgendas').removeClass('hide');
            $('#btnAbrirModalFecharAgenda').removeClass('hide');
        }
        else {
            $('#MaximizarCalendario').addClass('hide');
            $('#btnAbrirModalTodasAgendas').addClass('hide');
            $('#btnAbrirModalFecharAgenda').addClass('hide');
        }
    }, 1000);
}

function ConverterData(data) {
    if (data == null || data == "")
        return '';

    var ano = data.substring(0, 4);
    var mes = data.substring(5, 7);
    var dia = data.substring(8, 10);

    return ano + '-' + mes + '-' + dia;
}

function onlyNumericKeyPress(e) {
    var keyCode = e.which ? e.which : e.keyCode;

    if (!(keyCode >= 48 && keyCode <= 57)) {
        return false;
    }
}

function onlyTextKeyPress(e) {
    var keyCode = e.which ? e.which : e.keyCode;
    var input = e.target.value;

    var regex = /^[A-Za-zÀ-ÿ\s]*$/;
    var char = String.fromCharCode(keyCode);

    if (!regex.test(input + char)) {
        return false;
    }

    if (char === " " && input[input.length - 1] === " ") {
        return false;
    }

    return true;
}

function ConverterParaTimeSpan(horario) {
    if (!horario) return null;
    const partes = horario.split(":");
    const horas = partes[0];
    const minutos = partes[1];
    return `${horas}:${minutos}:00`;
}

function FecharTodosCardsAdministracao() {
    $('[id="alterarVisualizacaoCard"]').each(function () {
        var card = $(this).closest('.card');
        var cardBody = card.find('.card-body');
        var cardHeader = card.find('.card-header');
        var eye = card.find('.card-header i');

        if (cardBody.is(":visible")) {
            cardBody.slideUp(600);
            cardHeader.addClass('arredondar-lados');
            eye.removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });
}

function CopiarParaAreaDeTransferencia(texto) {
    navigator.clipboard.writeText(texto)
        .then(() => {
            console.log("Texto copiado para a área de transferência!");
        })
        .catch(err => {
            console.error("Erro ao copiar texto: ", err);
        });
}

function documentReady() {
    $(".only-numeric").bind("keypress", onlyNumericKeyPress);
    $(".only-text").bind("keypress", onlyTextKeyPress);
    $("body").delegate('#btnAbrirModalMeusAgendamentos', 'click', btnAbrirModalMeusAgendamentos_OnClick);
    $("body").delegate('#alterarVisualizacaoCard', 'click', alterarVisualizacaoCard_OnClick);
    $("body").delegate('#btnArmazenarTelefone', 'click', btnArmazenarTelefone_OnClick);
    $("body").delegate('#meuTelefoneReservas', 'keydown', function (e) {
        if (e.key === "Enter" || e.keyCode === 13) {
            btnArmazenarTelefone_OnClick();
        }
    });
    aplicarMascaras();
}

$(document).ready(documentReady);