const uri = "/User/login";
const login = () => {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const newUser = {
        Id: 0,
        Name: username,
        Password: password,
        Type: ""
    }
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newUser)
    }).then((response) => {
        if (response.status === 200) {
            return response.text();
        }
        else throw new Error("unauthorize");
    }).then((result) => {
        localStorage.setItem("token", result);
        window.location.href = "index.html";
    })
        .catch((err) => {
            console.log("error", err);
            alert(err);
        })
}