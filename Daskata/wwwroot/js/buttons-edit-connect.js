//Edit or connect bottons difference on self profile page
document.addEventListener('DOMContentLoaded', function () {
    var viewUserUsername = document.getElementById('viewUserUsername').innerText.trim();
    var loggedUserUsername = '@' + document.getElementById('loggedUserUsername').value;

    if (viewUserUsername === loggedUserUsername) {
        document.getElementById('connectButtonDesktop').innerText = 'Редактирай';
        document.getElementById('connectButtonDesktop').setAttribute('href', '/Profile/Edit');
        document.getElementById('connectButtonMobile').innerText = 'Редактирай';
        document.getElementById('connectButtonMobile').setAttribute('href', '/Profile/Edit');
    } else {
        document.getElementById('connectButtonDesktop').innerText = 'Свържете се!';
        document.getElementById('connectButtonDesktop').setAttribute('href', '#');
        document.getElementById('connectButtonMobile').innerText = 'Свържете се!';
        document.getElementById('connectButtonMobile').setAttribute('href', '#');
    }
});
