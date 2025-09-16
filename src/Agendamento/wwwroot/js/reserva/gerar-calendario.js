class CalendarWeek {
    constructor(data) {
        this.monday = data;
    }

    getWeekDays() {
        const weekDays = [];
        const dayLabels = ['SEG', 'TER', 'QUA', 'QUI', 'SEX', 'SAB', 'DOM'];
        let currentDate = new Date(this.monday);

        for (let i = 0; i < 7; i++) {
            weekDays.push({
                dayLabel: dayLabels[i],
                dayNumber: currentDate.getDate(),
                month: currentDate.getMonth(),
                year: currentDate.getFullYear(),
                data: new Date(currentDate)
            });
            currentDate.setDate(currentDate.getDate() + 1);
        }

        return weekDays;
    }
}

function renderWeek(week) {
    const container = document.querySelector('#conteudo-calendario');
    container.innerHTML = '';

    const leftArrow = document.createElement('i');
    leftArrow.className = 'arrow-icon2 left fa fa-angle-left';
    leftArrow.id = 'VoltarDataCalendario';
    leftArrow.style.cursor = 'pointer';
    leftArrow.style.paddingRight = '5px';
    leftArrow.style.fontSize = '20px';
    leftArrow.style.color = 'gray';
    container.appendChild(leftArrow);

    const dataAtual = new Date();
    dataAtual.setHours(0, 0, 0, 0);

    week.forEach(day => {
        const dayDiv = document.createElement('div');
        const dataUsada = day.data;

        if (dataUsada < dataAtual)
            dayDiv.className = 'desabilitar';

        else
            dayDiv.className = 'dia-semana';

        dayDiv.setAttribute('data-ano', day.year);
        dayDiv.setAttribute('data-mes', day.month);
        dayDiv.setAttribute('data-dia', day.dayNumber);
        dayDiv.innerHTML = `<div>${day.dayLabel}</div><b>${day.dayNumber}</b>`;
        container.appendChild(dayDiv);
    });

    const rightArrow = document.createElement('i');
    rightArrow.className = 'arrow-icon2 right fa fa-angle-right';
    rightArrow.id = 'AvancarDataCalendario';
    rightArrow.style.cursor = 'pointer';
    rightArrow.style.paddingLeft = '5px';
    rightArrow.style.fontSize = '20px';
    rightArrow.style.color = 'gray';
    container.appendChild(rightArrow);
}

function InicializarCalendarioComData(data) {
    const calendarWeek = new CalendarWeek(data);
    const week = calendarWeek.getWeekDays();
    renderWeek(week, data);

    $('#mes-ano-selecionado').text(obterMesAnoSelecionado(data));
}

function InicializarCalendario() {
    var segundaFeira = PegarASegundaFeiraDaSemanaAtual();

    const calendarWeek = new CalendarWeek(segundaFeira);
    const week = calendarWeek.getWeekDays();

    renderWeek(week, segundaFeira);
}

function PegarASegundaFeiraDaSemanaAtual() {
    let dataAtual = new Date();
    let diaDaSemana = dataAtual.getDay();
    let diasAteSegunda = (diaDaSemana + 6) % 7;
    let segundaFeira = new Date(dataAtual);
    segundaFeira.setDate(dataAtual.getDate() - diasAteSegunda);

    return segundaFeira;
}

function AvancarCalendario_OnClick() {
    var ultimoDiaSemanaAtual = $('.dia-semana').last();

    if (ultimoDiaSemanaAtual.length == 0)
        ultimoDiaSemanaAtual = $('.desabilitar').last();

    var ano = parseInt(ultimoDiaSemanaAtual.attr('data-ano'));
    var mes = parseInt(ultimoDiaSemanaAtual.attr('data-mes'));
    var dia = parseInt(ultimoDiaSemanaAtual.attr('data-dia'));

    var data = new Date(ano, mes, dia);
    data.setDate(data.getDate() + 1);

    InicializarCalendarioComData(data);
    SelecionarPrimeiraDataDisponivel();
}

function SelecionarPrimeiraDataDisponivel() {

    if ($('.dia-semana').length > 0)
        $('.dia-semana').eq(0).click();
}

function VoltarCalendario_OnClick() {
    var desabilitados = $('.desabilitar').first();

    if (desabilitados.length > 0)
        return;

    var ultimoDiaSemanaAtual = $('.dia-semana').first();

    var ano = parseInt(ultimoDiaSemanaAtual.attr('data-ano'));
    var mes = parseInt(ultimoDiaSemanaAtual.attr('data-mes'));
    var dia = parseInt(ultimoDiaSemanaAtual.attr('data-dia'));

    let dataAtual = new Date(ano, mes, dia);
    dataAtual.setDate(dataAtual.getDate() - 7);

    InicializarCalendarioComData(dataAtual);
    SelecionarPrimeiraDataDisponivel();
}

function documentReadyGerarCalendario() {
    $("body").delegate('#AvancarDataCalendario', 'click', AvancarCalendario_OnClick);
    $("body").delegate('#VoltarDataCalendario', 'click', VoltarCalendario_OnClick);
    InicializarCalendario();
}

$(document).ready(documentReadyGerarCalendario);