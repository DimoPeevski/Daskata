//Edit or connect bottons difference on preview profile page
document.addEventListener('DOMContentLoaded', function () {
    var viewUserUsername = document.getElementById('viewUserUsername').innerText.trim();
    var loggedUserUsername = '@' + document.getElementById('loggedUserUsername').value;

    if (viewUserUsername === loggedUserUsername) {
        document.getElementById('connectButtonDesktopPreview').innerText = 'Редактирай';
        document.getElementById('connectButtonDesktopPreview').setAttribute('href', '/Profile/Edit');
        document.getElementById('connectButtonMobilePreview').innerText = 'Редактирай';
        document.getElementById('connectButtonMobilePreview').setAttribute('href', '/Profile/Edit');
    } else {
        document.getElementById('connectButtonDesktopPreview').innerText = 'Свържете се!';
        document.getElementById('connectButtonDesktopPreview').setAttribute('href', '#');
        document.getElementById('connectButtonMobilePreview').innerText = 'Свържете се!';
        document.getElementById('connectButtonMobilePreview').setAttribute('href', '#');
    }
});
