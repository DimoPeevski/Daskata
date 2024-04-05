//Notification for create, edit or delete
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