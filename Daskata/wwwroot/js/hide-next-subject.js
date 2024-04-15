//Hide the next button if no subject was selected
document.addEventListener('DOMContentLoaded', function () {
    var subjectButtons = document.querySelectorAll('.subject-button');
    var subjectInput = document.querySelector('#subjectInput');
    var submitButton = document.querySelector('#submitButton');

    submitButton.disabled = true;

    subjectButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            subjectButtons.forEach(function (b) {
                b.classList.remove('button-create-user');
                b.classList.add('button-create-test');
            });

            button.classList.remove('button-create-test');
            button.classList.add('button-create-user');
            subjectInput.value = button.getAttribute('data-value');

            submitButton.disabled = false;
        });
    });
});