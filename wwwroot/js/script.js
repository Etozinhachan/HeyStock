const jwt_token_Header = "heystock-login-jwt-token";
const login_container = document.querySelector('.card-login');
const username_input = document.querySelector('#usuario');
const password_input = document.querySelector('#password');
const login_btn = document.querySelector('.btn-login');

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

const login_connection = new signalR.HubConnectionBuilder()
    .withUrl("/loginhub", { accessTokenFactory: async() => await get_token() })
    .build();


window.onload = async () => {
    await new Promise(r => setTimeout(r, 2000))
    console.log(`Connection: ${login_connection}}`)
    await startLoginConnection()
    /*await sendMessage("Eto_chan", "rawr")
    await joinGroup("Group1")
    await sendMessageToGroup("Group1", "Eto_chan", "rawr2") */
    console.log(getCurrentPath())
}




    async function startLoginConnection() {
        try {
            await login_connection.start();
           
        } catch (err) {
            console.error(err);
            setTimeout(() => startLoginConnection(), 5000); // Retry every 5 seconds
        }
    }

async function get_token(){

}

async function login(e){
    var name = document.getElementById('usuario').Value
    var pass = document.getElementById('password').Value

    console.log("Nome: " + name);
    console.log("pass: " + pass);
}

async function handle_login_submit(){

    console.log('rwar')
/*     const username_input = document.querySelector('input#usuario')
    const password_input = document.querySelector('input#password')
    console.log(username_input.value)
    console.log(password_input.value) */

    await login_connection.invoke('register_user', username_input.value, password_input.value);
}

login_connection.on("RegisteredMessage", (user, jwt) => {
    console.log(user)
    console.log(jwt)
})


login_btn.addEventListener('click', async () => { await handle_login_submit() });