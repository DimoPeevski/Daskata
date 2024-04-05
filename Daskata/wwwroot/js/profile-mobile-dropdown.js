// Profile mobile menu drop-down
document.addEventListener("DOMContentLoaded", function () {
    var profilePictureMobile = document.getElementById("profile-picture-mobile");
    var dropdownMenuMobile = document.getElementById("dropdown-menu-mobile");

    profilePictureMobile.addEventListener("click", function () {
        if (dropdownMenuMobile.style.display === "none" || dropdownMenuMobile.style.display === "") {
            dropdownMenuMobile.style.display = "block";
        } else {
            dropdownMenuMobile.style.display = "none";
        }
    });
});