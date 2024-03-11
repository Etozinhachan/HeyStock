const jwt_token_Header = "heystock-login-jwt-token";

var sleep = ms => new Promise(r => setTimeout(r, ms));


var setCookie = async (cname, cvalue, duration) => {
    const d = new Date();
    var days = duration.days,
        hours = duration.hours,
        minutes = duration.minutes,
        seconds = duration.seconds,
        miliseconds = duration.miliseconds
    if (days){
        d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000));
    }
    if (hours){
        d.setTime(d.getTime() + (hours * 60 * 60 * 1000));
    }
    if (minutes){
        d.setTime(d.getTime() + (minutes * 60 * 1000));
    }
    if (seconds){
        d.setTime(d.getTime() + (seconds * 1000));
    }
    if (miliseconds){
        d.setTime(d.getTime() + (miliseconds));
    }
    
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

var getCookie = (cname, ignoreCheck) => {
    if (ignoreCheck == undefined) {
        checkCookies();
    }
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

var checkCookies = () => {
    let username = getCookie("UserName", true);
    let pw = getCookie("passHash", true);
    let heystock_jwt = getCookie(jwt_token_Header, true);
    if (username == "" || pw == "" || heystock_jwt == "") {
        deleteCookie('UserName')
        deleteCookie('passHash')
        deleteCookie(jwt_token_Header)
        window.location.replace(`${window.location.href}`.replace('newChat.html', ''));
    }
}

var deleteCookie = name => {
    document.cookie = name + "=;expires=" + new Date(0).toUTCString()
}



function getCurrentPath(){
    const path_splitted = window.location.href.split("/")

    let host_path = ""
    let page_path = ""
    let paths = []
    path_splitted.pop()
    for (let i = 0; i < path_splitted.length; i++) {
        if (i <= 2){
            host_path += `${path_splitted[i]}/`
        }else if (i > 2){
            page_path += `${path_splitted[i]}/`
        }
    }
    paths.push(host_path)
    paths.push(page_path)
    return paths
}