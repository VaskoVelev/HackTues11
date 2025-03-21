// Show a specific section
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

// Default to showing 'HOME' on page load
document.addEventListener('DOMContentLoaded', () => {
    if (route == '/login') {
        showSection('loginSection');
    } else {
        showSection('HOME');
    }
        
    setTimeout(() => document.getElementById('flash-message').classList.add('hidden'), 7000);
});

// Download Game
function downloadGame() {
    const downloadLink = document.createElement('a');
    downloadLink.href = 'game.zip'; 
    downloadLink.download = 'STEAL_TO_HEAL_GAME.zip'; 
    downloadLink.click();
}

// Handle user inactivity (security feature)
let userIsHuman = false;
window.addEventListener('mousemove', () => userIsHuman = true);
window.addEventListener('keydown', () => userIsHuman = true);

// Login Functionality
function showLoginPopup() {
    showSection('loginSection');
}

// Login user
function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    // Check credentials (for demo purposes, hardcoded)
    if (username === 'user' && password === 'password123') {
        localStorage.setItem('loggedIn', 'true');
        showSection('HOME');  // Return to HOME page
        alert('Login successful!');
    } else {
        document.getElementById('loginError').textContent = 'Invalid credentials, please try again.';
    }
}

// Register user (for demo purposes)
function register() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    // Simulate user registration (store data in local storage, for demo purposes)
    localStorage.setItem('user_' + username, password);
    alert('Registration successful! Please log in now.');
}

// Logout functionality
function logout() {
    localStorage.removeItem('loggedIn');
    alert('Logged out successfully!');
    showSection('HOME'); // Go back to HOME
}

// Check if user is logged in and show relevant UI
document.addEventListener('DOMContentLoaded', () => {
    if (localStorage.getItem('loggedIn') === 'true') {
        document.querySelector('nav button:last-child').style.display = 'inline'; // Show Logout button
    }
});



