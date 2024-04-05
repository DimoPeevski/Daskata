// Reduce exam preview title and description
document.addEventListener('DOMContentLoaded', function () {
    var examTitles = document.querySelectorAll('.exam-title');
    var examDescriptions = document.querySelectorAll('.exam-description');

    for (var i = 0; i < examTitles.length; i++) {
        examTitles[i].textContent = truncateText(examTitles[i].textContent, 40);
    }

    for (var j = 0; j < examDescriptions.length; j++) {
        examDescriptions[j].textContent = truncateText(examDescriptions[j].textContent, 137);
    }
});

function truncateText(text, maxLength) {
    return text.length > maxLength ? text.slice(0, maxLength - 3) + '...' : text;
}