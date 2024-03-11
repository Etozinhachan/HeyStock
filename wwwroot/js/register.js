
const login_container = document.querySelector('.card-login');
const username_input = document.querySelector('#usuario');
const password_input = document.querySelector('#password');
const register_btn = document.querySelector('.btn-login');

let possible_to_submit = true;



const login_connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/loginhub")
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

async function register(e) {
    var name = document.getElementById('usuario').Value
    var pass = document.getElementById('password').Value

    console.log("Nome: " + name);
    console.log("pass: " + pass);
}

async function handle_login_submit() {
    if (username_input.value.length < 3){
        //username_input.setCustomValidity("Invalid field.");
        username_input.reportValidity();
        return;
    }
    if (password_input.value.length < 8){
        //password_input.setCustomValidity("Porfavor insira mais doque 8 caracteres .");
        password_input.reportValidity();
        return;
    }
    if (!possible_to_submit){
        //console.log('meow :c')
        return;
    }

    possible_to_submit = false;

    console.log('rwar')
    /*     const username_input = document.querySelector('input#usuario')
        const password_input = document.querySelector('input#password')
        console.log(username_input.value)
        console.log(password_input.value) */

        const JsonString = {
            "UserName": `${username_input.value}`,
            "passHash": `${password_input.value}`
        } 
    
        const json = JSON.stringify(JsonString)
    
        await login_connection.invoke('register_user', json);
}

login_connection.on("UserRegistered", async (jwt) => {
    await setCookie(jwt_token_Header, jwt, { minutes: 30 })
    console.log("meow registered")
    console.log(getCookie(jwt_token_Header, true))
    //window.location.href = `${getCurrentPath()[0]}vendas.html`;
    window.location.href = `${getCurrentPath()[0]}testeLista.html`;
});

login_connection.on("LoginError", (error_message) => {
    console.error(error_message)
    possible_to_submit = true;
});


register_btn.addEventListener('click', async () => { await handle_login_submit() });