// In the client (JavaScript)

const counter_div = document.querySelector('#counter_div')

var sleep = ms => new Promise(r => setTimeout(r, ms));

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

window.onload = async () => {
    await new Promise(r => setTimeout(r, 2000))
    console.log(`Connection: ${connection}}`)
    await startConnection()
    await sendMessage("Eto_chan", "rawr")
    await joinGroup("Group1")
    await sendMessageToGroup("Group1", "Eto_chan", "rawr2")
}

connection.onclose(async () => {
    // Handle reconnection
    await startConnection();
});

async function startConnection() {
    try {
        await connection.start();
    } catch (err) {
        console.error(err);
        setTimeout(() => startConnection(), 5000); // Retry every 5 seconds
    }
}



connection.on("ReceiveMessage", (user, message) => {
    console.log(user)
    console.log(message)
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



counter_div.querySelector('button').addEventListener('click', async () => { await buttonHandler(); })