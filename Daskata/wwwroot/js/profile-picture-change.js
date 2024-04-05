//Profile image change
document.getElementById('profilePicture').addEventListener('change', function () {
    var file = this.files[0];
    if (file) {
        var reader = new FileReader();
        reader.onload = function () {
            document.getElementById('profileImage').src = reader.result;
        }
        reader.readAsDataURL(file);
    }
});

function showNotification() {
    var notification = document.getElementById("userEditedNotification");
    notification.classList.add("show");

    setTimeout(function () {
        notification.classList.remove("show");
    }, 1000);
}

var form = document.getElementById("editForm");

form.addEventListener("submit", function (event) {
    event.preventDefault();

    showNotification();

    setTimeout(function () {
        form.submit();
    }, 1000);
});