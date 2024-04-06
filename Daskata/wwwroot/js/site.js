/* ************************************************************************
 * Author: Dimo Peevski                                                   *
 * Version: 1.0                                                           *
 * Date: 01.03.2024                                                       *
 * ************************************************************************/

//Logout notification pop-up desktop
document.addEventListener("DOMContentLoaded", function () {
    var logoutButtonDesktop = document.getElementById("logoutLinkDesktop");

    logoutButtonDesktop.addEventListener("click", function () {
        alert("Ще излезете от профила си!");
    });
});


//Logout notification pop-up mobile
document.addEventListener("DOMContentLoaded", function () {
    var logoutButtonMobile = document.getElementById("logoutLinkMobile");

    logoutButtonMobile.addEventListener("click", function () {
        alert("Ще излезете от профила си!");
    });
});


// Profile menu drop-down desktop
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


// Profile menu drop-down mobile
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

