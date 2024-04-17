// Confirmation for question deletion
document.querySelectorAll(".delete-question").forEach(function (form) {
    form.addEventListener("submit", function (event) {
        var confirmation = confirm("Сигурни ли сте, че искате да изтриете този въпрос?");
        if (!confirmation) {
            event.preventDefault();
        }
    });
});

// Confirmation for answer deletion
document.querySelectorAll(".delete-answer").forEach(function (form) {
    form.addEventListener("submit", function (event) {
        var confirmation = confirm("Сигурни ли сте, че искате да изтриете този отговор?");
        if (!confirmation) {
            event.preventDefault();
        }
    });
});