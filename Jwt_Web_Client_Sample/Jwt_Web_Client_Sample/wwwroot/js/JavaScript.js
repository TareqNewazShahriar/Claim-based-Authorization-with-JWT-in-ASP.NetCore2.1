const apiBaseUrl = document.getElementById('apiUrl').getAttribute('content') + '/';

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


function ajax(url, data, successCallback, method = 'GET', apiText = true) {
    var xhr = new XMLHttpRequest();
    
    xhr.open(method, apiBaseUrl + (apiText ? 'api/' :'') + url, true);

    if (method == 'POST')
        xhr.setRequestHeader("Content-Type", "application/json");

    var token = getCookie('token');
    if (token)
        xhr.setRequestHeader('Authorization', 'Bearer ' + token);
    xhr.setRequestHeader('Access-Control-Allow-Origin', '*');

    xhr.onload = successCallback || function defaultSuccessCallback(a, b) {
        log('from api: ' + this.responseText + ' || Status: ' + this.status);
        if (this.status == 401) {
            alert('Session ended.');
            window.location.reload(true);
        }
    };
    xhr.onerror = function (a, b) {
        log('statusText: ' + this.statusText + ' || status: ' + this.status);
    }

    xhr.send(JSON.stringify(data));
}

function login() {

    ajax('account/login',
        { email: 'tariq.information@gmail.com', password: 'admin' },
        null,
        'POST'
    );

    return false;
}

function log(text) {
    console.log(text);

    let panel = document.querySelector('#log');
    if (panel)
        panel.innerHTML = panel.innerHTML + text + '\n';
}
