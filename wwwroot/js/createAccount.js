const btn = document.getElementById("btn")
const name = document.getElementById("user")
const email = document.getElementById("email")
const pass = document.getElementById("password")


btn.addEventListener("click", (e) => validate(e));

function validate(e){
    e.preventDefault();

    if(name.value === ""){
        alert("burro")
    }else if (email.value === ""){
        alert("burro")
    }else if (pass.value === ""){
        alert("burro")
    }else{
        alert("clap")
    }
}
