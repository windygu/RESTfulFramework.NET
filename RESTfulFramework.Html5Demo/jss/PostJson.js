$.postJSON = function (url, data, jqXHR) {
    return $.post(url, JSON.stringify(data), jqXHR);
};