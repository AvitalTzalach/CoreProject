//localStorage.removeItem("token");
const uri = '/Jewelry';
let jewelryList = [];
let currentType = null;
let currentUserId = null;
let token = localStorage.getItem("token");
token = JSON.parse(token);

isTokenExpired = (token) => {
    const payloadBase64 = token.split(".")[1]; 
    const payloadJSON = atob(payloadBase64); 
    const payload = JSON.parse(payloadJSON); 
    const currentTime = Math.floor(Date.now() / 1000); 
    return payload.exp < currentTime; // true אם פג תוקף
}

if (token == null)
    window.location.href = "login.html";
else if (isTokenExpired(token.token)) {
    localStorage.removeItem("token"); // מחיקת הטוקן
    window.location.href = "login.html"; // מעבר לדף התחברות
}
else{
    const tokenParts = token.split(".");
    const payload = JSON.parse(atob(tokenParts[1])); // פענוח ה-Payload
    currentType = payload.Type;
    currentUserId = payload.UserId;
}

const logOut = () => {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

if (currentType === 'Admin')
{
    document.getElementById("Display-users").hidden = false;
    document.getElementById("add-userId").hidden = false;
}
else
    document.getElementById("Display-details-user").hidden = false;

// פונקציה להפניה לדף המשתמשים
const redirectToUsersPage = () => {
    window.location.href = "users.html"; // שנה את זה לכתובת הדף שלך
};

const addJewel = (event) => {
    event.preventDefault();
    let userId = document.getElementById('add-userId');
    if(userId.hidden == false)
        userId = document.getElementById('add-userId');
    else
        userId = parseInt(currentUserId, 10);
    const nameJewel = document.getElementById('add-name');
    const pricejewel = document.getElementById('add-price');
    const categoryJewel = document.getElementById('jewelry-select');

    const indexCategory = categoryJewel.selectedIndex;

    const newJewel = {
        id: 0,
        userId: userId.value.trim(),
        name: nameJewel.value.trim(),
        price: pricejewel.value.trim(),
        category: indexCategory
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}`
        },
        body: JSON.stringify(newJewel)
    })
        .then(response => response.json())
        .then(() => {
            getJewelry();
            nameJewel.value = '';
            pricejewel.value = '';
            categoryJewel.value = '';
            userId.value = '';
        })
        .catch(error => console.error('Unable to add jewel.', error));

}

const displayEditForm = (id) => {
    const item = jewelryList.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-price').value = item.price;
    document.getElementById('edit-jewelry-select').selectedIndex = item.category;
    document.getElementById('editForm').style.display = 'block';
}

const updateJewel = (event) => {
    event.preventDefault();
    const jewelId = document.getElementById('edit-id').value;
    const updatedJewel = {
        id: parseInt(jewelId, 10),
        name: document.getElementById('edit-name').value.trim(),
        price: document.getElementById('edit-price').value.trim(),
        category: document.getElementById('edit-jewelry-select').selectedIndex
    };

    fetch(`${uri}/${jewelId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}`
        },
        body: JSON.stringify(updatedJewel)
    })
        .then(() => getJewelry())
        .catch(error => console.error('Unable to update jewel.', error));

    closeInput();

    return false;
}

const closeInput = () => {
    document.getElementById('editForm').style.display = 'none';
}

const deleteJewel = (id) => {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token.token}` // דוגמה לשימוש ב-Token
        }
    })
        .then(() => getJewelry())
        .catch(error => console.error('Unable to delete jewel.', error));
}



const _displayItems = (data) => {

    const tBody = document.getElementById('Jewelry-table');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(item => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteJewel(${item.id})`);

        let dispalayJewelInRow = tBody.insertRow();

        let tdId = dispalayJewelInRow.insertCell(0);
        tdId.innerHTML = item.id;

        let tdUserId = dispalayJewelInRow.insertCell(1);
        tdUserId.innerHTML = item.userId;

        let tdName = dispalayJewelInRow.insertCell(2);
        tdName.innerHTML = item.name;

        let tdPrice = dispalayJewelInRow.insertCell(3);
        tdPrice.innerHTML = item.price;

        let tdCategory = dispalayJewelInRow.insertCell(4);
        tdCategory.innerHTML = document.getElementById("edit-jewelry-select")[item.category].innerHTML;

        let tdEdit = dispalayJewelInRow.insertCell(5);
        tdEdit.appendChild(editButton);

        let tdDelete = dispalayJewelInRow.insertCell(6);
        tdDelete.appendChild(deleteButton);
    });

    jewelryList = data;
}


const getJewelry = () => {
    //לקבלת מערך הפריטים GET גישה לפונקציית 
    fetch(uri, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token.token}` // הוספת ה-Token ל-Header
        }
    })
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}