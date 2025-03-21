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
    showSection('HOME');
});



function downloadGame() {
    const downloadLink = document.createElement('a');
    downloadLink.href = 'game.zip'; 
    downloadLink.download = 'STEAL_TO_HEAL_GAME.zip'; 
    downloadLink.click();
}



let userIsHuman = false;

window.addEventListener('mousemove', () => userIsHuman = true);
window.addEventListener('keydown', () => userIsHuman = true);

setTimeout(() => {
    if (!userIsHuman) {
        sessionStorage.setItem('accessDenied', 'true');
        location.reload();
    }
}, 5000);

if (sessionStorage.getItem('accessDenied') === 'true') {
    document.body.innerHTML = '<h1 class="access-denied">Access Denied</h1>';
    sessionStorage.removeItem('accessDenied');
}

