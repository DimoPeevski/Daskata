document.addEventListener("DOMContentLoaded", function () {
    var profilePictureDesktop = document.getElementById("profile-picture-desktop");
    var dropdownMenuDesktop = document.getElementById("dropdown-menu-desktop");

    profilePictureDesktop.addEventListener("click", function () {
        if (dropdownMenuDesktop.style.display === "none" || dropdownMenuDesktop.style.display === "") {
            dropdownMenuDesktop.style.display = "block";
        } else {
            dropdownMenuDesktop.style.display = "none";
        }
    });
});
