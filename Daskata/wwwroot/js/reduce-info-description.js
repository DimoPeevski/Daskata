// Reduce info preview description
document.addEventListener('DOMContentLoaded', function () {
    var infoDescriptions = document.querySelectorAll('.info-description');


    for (var j = 0; j < infoDescriptions.length; j++) {
        infoDescriptions[j].textContent = truncateText(infoDescriptions[j].textContent, 137);
    }
});

function truncateText(text, maxLength) {
    return text.length > maxLength ? text.slice(0, maxLength - 3) + '...' : text;

}

function openProfilePage(username) {
    window.location.href = '/Profile/Preview/' + username;
}
