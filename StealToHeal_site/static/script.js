function showSection(sectionId) {
    const sections = document.querySelectorAll('section');
    sections.forEach(section => {
        section.classList.add('hidden');
    });

    const sectionToShow = document.getElementById(sectionId);
    if (sectionToShow) {
        sectionToShow.classList.remove('hidden');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    if (typeof route != 'undefined' && route == '/login') {
        showSection('loginSection');
    } else if (typeof route != 'undefined' && route == '/register') {
        showSection('registerSection');
    } else {
        showSection('HOME');
    }
        
    setTimeout(() =>{
        document.getElementById('flash-message')?.classList?.add('hidden');
        const flashMessageElements = document.getElementsByClassName("flash-message");
        for (let item of flashMessageElements) {
            console.log(flashMessageElements);
            item.classList.add('hidden');
        }
    }, 7000);
});

let userIsHuman = false;
window.addEventListener('mousemove', () => userIsHuman = true);
window.addEventListener('keydown', () => userIsHuman = true);

function showLoginPopup() {
    showSection('loginSection');
}

function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    if (username === 'user' && password === 'password123') {
        localStorage.setItem('loggedIn', 'true');
        showSection('HOME');  
        alert('Login successful!');
    } else {
        document.getElementById('loginError').textContent = 'Invalid credentials, please try again.';
    }
}

function register() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    localStorage.setItem('user_' + username, password);
    alert('Registration successful! Please log in now.');
}

function logout() {
    window.location.href = '/logout';
}

document.addEventListener('DOMContentLoaded', () => {
    if (localStorage.getItem('loggedIn') === 'true') {
        document.querySelector('nav button:last-child').style.display = 'inline'; 
    }
});

function errordownload() {
    window.location.href = '/errordownload';
}


