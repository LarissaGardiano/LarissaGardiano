function MostrarImagemSelecionada(id) {
    $('[id^=imgClick]').hide();
    $('#imgClick' + id).show();

    $('[id^=btnClick]').removeClass('border-gray');
    $('#btnClick' + id).addClass('border-gray');
}

function btnClickImage_OnClick() {
    const id = $(this).attr('id').replace('btnClick', '');
    MostrarImagemSelecionada(id);
}

function documentLoadingImages() {
    $("body").delegate('[id^=btnClick]', 'click', btnClickImage_OnClick);

    MostrarImagemSelecionada(1);
}

$(document).ready(documentLoadingImages);