
function apiPost(dados, url) {
    return $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(dados),
        contentType: "application/json; charset=utf-8"
    });
}

function apiPostData(formData, url) {
    return $.ajax({
        type: "POST",
        url: url,
        data: formData,
        processData: false,
        contentType: false
    });
}

function apiUpdate(dados, url) {
    return $.ajax({
        type: "PUT",
        url: url,
        data: JSON.stringify(dados),
        contentType: "application/json; charset=utf-8"
    });
}

function apiUpdateData(formData, url) {
    return $.ajax({
        type: "PUT",
        url: url,
        data: formData,
        processData: false,
        contentType: false
    });
}

function apiGet(id, url) {
    return $.ajax({
        type: "GET",
        url: url.replace("_id", id),
        data: {},
        contentType: "application/json; charset=utf-8"
    });
}

function apiGet(url) {
    return $.ajax({
        type: "GET",
        url: url,
        data: {},
        contentType: "application/json; charset=utf-8"
    });
}

function apiDelete(url) {
    return $.ajax({
        type: "DELETE",
        url: url,
        data: {},
        contentType: "application/json; charset=utf-8"
    });
}