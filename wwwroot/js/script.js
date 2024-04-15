const login_container = document.querySelector('.card-login');
const username_input = document.querySelector('#usuario');
const password_input = document.querySelector('#password');
const login_btn = document.querySelector('.btn-login');


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

async function login(e) {
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
        console.log('meow :c')
        return;
    }
    possible_to_submit = false;

    let usingEmail = username_input.value.includes("@") || username_input.value.includes(".")

    //console.log('rwar')
    /*     const username_input = document.querySelector('input#usuario')
        const password_input = document.querySelector('input#password')
        console.log(username_input.value)
        console.log(password_input.value) */
    const JsonString = {
        "usernameOrEmail": `${username_input.value}`,
        "passHash": `${password_input.value}`,
        "usingEmail": `${usingEmail}`
    
    } 

    const json = JSON.stringify(JsonString)

    await login_connection.invoke('login_user', json);
}


const showError = (input, message) => {
    // get the form-field element
    input.value = ""
    const formField = input.parentElement;
    // add the error class
    formField.classList.remove('success');
    formField.classList.add('error');

    // show the error message
    const error = formField.querySelector('small');
    error.textContent = message;
};

const showSuccess = (input) => {
    // get the form-field element
    const formField = input.parentElement;

    // remove the error class
    formField.classList.remove('error');
    formField.classList.add('success');

    // hide the error message
    const error = formField.querySelector('small');
    error.textContent = '';
};


login_connection.on("UserLogin", async (jwt) => {
    await setCookie(jwt_token_Header, jwt, { minutes: 30 })
    //console.log("meow login")
    //console.log(getCookie(jwt_token_Header, true))
    //window.location.href = `${getCurrentPath()[0]}vendas.html`;
    window.location.href = `${getCurrentPath()[0]}testeLista.html`;
});

login_connection.on("LoginError", (error_message, wrong_inputs, inputs_to_show_error) => {
    console.error(error_message)
    possible_to_submit = true;
    let inputs_to_error = inputs_to_show_error.split(',')
    console.log(inputs_to_error)
    console.log(wrong_inputs)
    inputs_to_error.forEach(element => {
        if (element == "username_input" || element == "email_input" || element == "all"){
            showError(username_input, '')
        }
        if(element == "password_input" || element == "all"){
            showError(password_input, '')
        }
    });
    if (wrong_inputs.includes("password")){
        showError(password_input, error_message)
    }
    if (wrong_inputs.includes("username") || wrong_inputs.includes("email")){
        showError(username_input, error_message)
    }
    //password_input.
});


login_btn.addEventListener('click', async () => { await handle_login_submit() });