const open_dialog_btn = document.querySelector(".open_dialog")
const dialog = document.querySelector("dialog")

function open_dialog_event_handler(){
    dialog.showModal()
}

open_dialog_btn.addEventListener("click", () => { open_dialog_event_handler() })