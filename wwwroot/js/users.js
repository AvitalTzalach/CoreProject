let token = localStorage.getItem("token");
token = JSON.parse(token);
if (token == null)
    window.location.href = "login.html";
const uri = '/User';
let UsersList = [];

const addUser = (event) => {
    event.preventDefault();
    const name = document.getElementById('add-name');
    const password = document.getElementById('add-password');
    const type = document.getElementById('add-users-select');

    const newUser = {
        id: 0,
        name: name.value.trim(),
        password: password.value.trim(),
        type: type.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}`
        },
        body: JSON.stringify(newUser)
    })
        .then(response => response.json())
        .then(() => {
            getUsers();
            name.value = '';
            password.value = '';
            type.value = '';
        })
        .catch(error => alert('Unable to add user.', error));

}

const displayEditForm = (id) => {
    const item = UsersList.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-password').value = item.password;
    document.getElementById('edit-users-select').selectedIndex = item.type;
    document.getElementById('edit-users-select').value = item.type;
    document.getElementById('editForm').style.display = 'block';
}

const updateUser = (event) => {
    event.preventDefault();
    const userId = document.getElementById('edit-id').value.trim();
    const updatedUser = {
        id: parseInt(userId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value,
        type: document.getElementById('edit-users-select').value
    };

    fetch(`${uri}/${userId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}`
        },
        body: JSON.stringify(updatedUser)
    })
    .then(response => {
        if (!response.ok) {
            // אם הסטטוס הוא 401 או כל סטטוס אחר של שגיאה
            throw new Error('Unauthorized');
        }
        return response.json(); // במקרה של הצלחה, אם יש תוכן
    })
        .then(() => getUsers())
        .catch(error => alert(`Unable to update user, ${error.message}`));

    closeInput();

    return false;
}

const closeInput = () => {
    document.getElementById('editForm').style.display = 'none';
}

const deleteUser = (id) => {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token.token}` // דוגמה לשימוש ב-Token
        }
    })
        .then(() => getUsers())
        .catch(error => alert('Unable to delete user.', error));
}


const _displayItems = (data) => {

    const tBody = document.getElementById('Users-table');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(item => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${item.id})`);

        let dispalayUserInRow = tBody.insertRow();

        let tdId = dispalayUserInRow.insertCell(0);
        tdId.innerHTML = item.id;

        let tdName = dispalayUserInRow.insertCell(1);
        tdName.innerHTML = item.name;

        let tdPassword = dispalayUserInRow.insertCell(2);
        tdPassword.innerHTML = item.password;

        let tdType = dispalayUserInRow.insertCell(3);
        tdType.innerHTML = item.type;

        let tdEdit = dispalayUserInRow.insertCell(4);
        tdEdit.appendChild(editButton);

        let tdDelete = dispalayUserInRow.insertCell(5);
        tdDelete.appendChild(deleteButton);
    });

    UsersList = data;
}

const getUserInfo = (token) => {
    const tokenParts = token.split(".");
    const payload = JSON.parse(atob(tokenParts[1])); // פענוח ה-Payload
    return {
        userType: payload.Type,
        userId: payload.UserId
    };
};



const getUsers = () => {
    const userInfo = getUserInfo(token.token);
    const requestUri  = userInfo.userType === "Admin" ? `${uri}` : `${uri}/${userInfo.userId}`;
    //לקבלת מערך הפריטים GET גישה לפונקציית 
    fetch(requestUri , {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token.token}` // הוספת ה-Token ל-Header
        }
    })
        .then(response => response.json())
        .then(data => { const usersList = Array.isArray(data) ? data : [data];
            _displayItems(usersList);})
        .catch(error => console.error('Unable to get items.', error));

}
