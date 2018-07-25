const apiBaseUrl = 'https://localhost:44393/';

function setCookie(c_name, value, exdays = 1)
{
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function getCookie(c_name)
{
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name)
        {
            return unescape(y);
        }
    }
}


function ajax(url, data, successCallback, method = 'GET') {
    var xhr = new XMLHttpRequest();

    xhr.open(method, apiBaseUrl + url, true);

    if (method == 'POST')
        xhr.setRequestHeader("Content-Type", "application/json");

    var token = getCookie('token');
    if (token)
        xhr.setRequestHeader('Authorization', 'Bearer ' + token);

    xhr.onload = successCallback || function defaultSuccessCallback(a, b) {
        log('from api: ' + this.responseText);
    };
    xhr.onerror = function (a, b) {
        log(this.statusText);
    }

    xhr.send(JSON.stringify(data));
}

function log(text) {
    document.querySelector('#log').innerHTML = document.querySelector('#log').innerHTML + '\n' + text;
}

function login() {

    ajax('/account/getauthenticated',
        { username: 'aa', Password: 'aa', email: 'aa@aa' },
        function (a, b) {
            setCookie('token', this.responseText);
            log('token acquired');
        },
        'POST'
    );

    return false;
}