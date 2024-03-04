// In the client (JavaScript)
const jwt_token_Header = "heystock-login-jwt-token";
const counter_div = document.querySelector('#counter_div')
const input_section = document.querySelector('.input_section')
const message_section = document.querySelector('.message_section')
const dialog_opener = document.querySelector('.dialog_opener')
const dialog = document.querySelector('.dialog')
const register_form = document.querySelector('#register')
const login_form = document.querySelector('#login')


const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub", { accessTokenFactory: () => this.loginToken })
    .build();

const login_connection = new signalR.HubConnectionBuilder()
.withUrl("/loginhub", { accessTokenFactory: () => this.loginToken })
.build();

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


window.onload = async () => {
    await new Promise(r => setTimeout(r, 2000))
    console.log(`Connection: ${connection}}`)
    await startLoginConnection()
    await startConnection()
    /*await sendMessage("Eto_chan", "rawr")
    await joinGroup("Group1")
    await sendMessageToGroup("Group1", "Eto_chan", "rawr2") */
    console.log(getCurrentPath())
}

/* connection.onclose(async () => {
    // Handle reconnection
    await startConnection();
}); */

async function startConnection() {
    try {
        await connection.start();
       
    } catch (err) {
        console.error(err);
        setTimeout(() => startConnection(), 5000); // Retry every 5 seconds
    }
}

async function startLoginConnection() {
    try {
        await login_connection.start();
       
    } catch (err) {
        console.error(err);
        setTimeout(() => startLoginConnection(), 5000); // Retry every 5 seconds
    }
}



connection.on("ReceiveMessage", (user, message) => {
    console.log(user)
    console.log(message)
    let div_class = ""
    if (user == "Eto_chan"){
        div_class = "user-row_1"
    }else{
        div_class = "user-row_2"
    }
    message_section.innerHTML += `<div class="${div_class}">
    <h1>${user}</h1>
    <p>${message}</p>
</div>`
    
});
// Send a message

async function sendMessage(user, message){
    await connection.invoke("SendMessage", user, message).catch(err => console.error(err));
}

async function joinGroup(group){
    await connection.invoke("JoinGroup", group).catch(err => console.error(err));
}

async function sendMessageToGroup(group, user, message){
    await connection.invoke("SendMessageToGroup", group, user, message).catch(err => console.error(err));
}

//connection.invoke("SendMessage", user, message).catch(err => console.error(err));

// In the client (JavaScript)
// Join a group
//connection.invoke("JoinGroup", "Group1").catch(err => console.error(err));

//message = "rawr2"

// Send a message to the group
//connection.invoke("SendMessageToGroup", "Group1", user, message).catch(err => console.error(err));
connection.on("ReceiveCounterMessage", (user, new_number) => {
    console.log(user)
    counter_div.querySelector('p').textContent = `${new_number}`
});

async function incrementCounter(user, current_number){
    await connection.invoke("incrementCounter", user, current_number).catch(err => console.error(err));
}



async function buttonHandler(){
    //counter_div.querySelector('p').textContent = `${Number(counter_div.querySelector('p').textContent) + 1}`
    await incrementCounter("Eto_chan", Number(counter_div.querySelector("p").textContent))
}

async function messageButtonHandler(){
    user = input_section.querySelector('#user_input').value
    message = input_section.querySelector('#message_input').value
    await sendMessage(user, message)
}

login_connection.on("RegisteredMessage", (user, jwt) => {
    console.log(user)
    console.log(jwt)
})

async function handle_register_submit(){

    console.log('rwar')

    const formData = new FormData(register_form)

    await login_connection.invoke('register_user', formData.get('UserName'), formData.get('passHash'))
}

async function handle_login_submit(){

    const formData = FormData(login_form)


}

counter_div.querySelector('button').addEventListener('click', async () => { await buttonHandler(); });
input_section.querySelector('button').addEventListener('click', async() => { await messageButtonHandler(); });
dialog_opener.addEventListener('click', () => { dialog.showModal() })
register_form.addEventListener('submit', async() => { await handle_register_submit() } )
login_form.addEventListener('submit', async() => { await handle_login_submit() } )