const lista = document.querySelector('.lista');
const tableBody = document.querySelector('.user-list-table-body')

/* const teste_connection = new signalR.HubConnectionBuilder()
    .withUrl("/testeHub", { accessTokenFactory: () => getCookie(jwt_token_Header, true)})
    .build(); */

const login_connection = new signalR.HubConnectionBuilder()
    .withUrl("/loginhub", { accessTokenFactory: () => getCookie(jwt_token_Header, true) })
    .build();


window.onload = async () => {
    await startLoginConnection()
    await new Promise(r => setTimeout(r, 2000));
    login_connection.invoke('refresh_users')
    /* await startTesteConnection()

    await new Promise(r => setTimeout(r, 2000));
    await teste_connection.invoke('refresh_users') */
    console.log(getCookie(jwt_token_Header, true))
}

/* async function startTesteConnection() {
    try {
        await teste_connection.start();

    } catch (err) {
        console.error(err);
        setTimeout(() => startTesteConnection(), 5000); // Retry every 5 seconds
    }
} */


async function startLoginConnection() {
    try {
        await login_connection.start();

    } catch (err) {
        console.error(err);
        setTimeout(() => startLoginConnection(), 5000); // Retry every 5 seconds
    }
}

login_connection.on("RefreshUserList", async () => {
    await login_connection.invoke('refresh_users');
});

/* teste_connection.on("RefreshedUserList", (users) => {
    console.log(users)
}) */

login_connection.on("RefreshedUserList", (users) => {
    console.log(users)
    lista.innerHTML = ""
    users.forEach(user => {
        let divElement = document.createElement("div")
        let pElement = document.createElement("p")
        pElement.textContent = `${user.id}|${user.isAdmin}|${user.userName}`
        divElement.appendChild(pElement)
        lista.appendChild(divElement)
    });

    tableBody.innerHTML = ""

    users.forEach(user => {
        const row = buildRow(user)
        tableBody.append(row)
    });
}) 


function buildRow(user) {
    const row = document.createElement('tr');
    const cellUsername = document.createElement('td')
    const cellId = document.createElement('td')
    const cellIsAdmin = document.createElement('td')
/*     const cellButons = document.createElement('td')
    const editButton = document.createElement('button')
    const deleteButton = document.createElement('button') */


    cellUsername.textContent = `${user.userName}`
    cellId.textContent = `${user.id}`
    cellIsAdmin.textContent = `${user.isAdmin}`

/*     editButton.className = 'EditButton'
    editButton.textContent = 'Edit'

    deleteButton.className = 'DeleteButton'
    deleteButton.textContent = 'Delete'

    row.id = `_${cellId.textContent}`

    editButton.addEventListener('click', async () => await editButtonHandler(row.id))
    deleteButton.addEventListener('click', async () => await deleteButtonHandler(row.id))

    cellButons.append(editButton)
    cellButons.append(deleteButton) */
    row.append(cellId)
    row.append(cellUsername)
    row.append(cellIsAdmin)
/*     row.append(cellButons) */
    return row;
}