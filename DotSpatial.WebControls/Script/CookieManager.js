function writeCookie(name, value, duration) {
    var expiration = new Date();
    var now = new Date();
    expiration.setTime(now.getTime() + (parseInt(duration) * 60000));
    document.cookie = name + '=' + escape(value) + '; expires=' + expiration.toGMTString() + '; path = /';
}

function readCookie(name) {
    if (document.cookie.length > 0) {
        var start = document.cookie.indexOf(name + "=");
        if (start != -1) {
            start = start + name.length + 1;
            var end = document.cookie.indexOf(";", start);
            if (end == -1) end = document.cookie.length;
            return unescape(document.cookie.substring(start, end));
        } else {
            return "";
        }
    }
    return "";
}

function deleteCookie(name) {
    writeCookie(name, '', -1);
}

function verifyCookie() {
    document.cookie = 'verifyCookie';
    var testcookie = (document.cookie.indexOf('verifyCookie') != -1) ? true : false;
    return testcookie;
}
