
function bloqueioDeTela(bloquear) {
    $(document).ready(function () {
        if (bloquear === true) {
            var domMessage = $('#domMessage');

            if (domMessage.length > 0) {
                domMessage.show();
                $('#domMessage').removeClass('hide');
                $.blockUI({ message: domMessage });
            }
        } else {
            $.unblockUI();
            $('#domMessage').hide();
            $('#domMessage').addClass('hide');
        }
    });
}

function InicializarDomMessage() {
    if ($('#domMessage').length === 0) {
        $('body').append(`
                <div id="domMessage" class="hide">
                    <img alt="Loading" src="../../img/loading.png" id="logoImg" width="60" />
                </div>`);
    }
}

function InicializarLoadingAjax() {
    $(document).ajaxStart(function () {
        if ($("#modal-credito-pagamento").is(":visible") || $("#modal-pix-pagamento").is(":visible"))
            return;

        bloqueioDeTela(true);
    });

    $(document).ajaxStop(function () {
        bloqueioDeTela(false);
    });

    $(document).ajaxComplete(function myErrorHandler(event, xhr, ajaxOptions, thrownError) {
        if (xhr.status == '401' || xhr.status == '405') {
            window.location.reload();
        }
        else if (xhr.status == '409' || xhr.status == '503' || xhr.status == '400' || xhr.status == '500') {
            bloqueioDeTela(false);
            const jsonObject = JSON.parse(xhr.responseText);
            console.log(jsonObject.Mensagens);
        }
    });
}

function documentLoadingAjax() {
    InicializarLoadingAjax();
    InicializarDomMessage();
}

$(document).ready(documentLoadingAjax);