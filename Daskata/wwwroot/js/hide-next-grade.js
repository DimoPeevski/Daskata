//Hide the next button if no grade was selected
document.addEventListener('DOMContentLoaded', function () {
    var circles = document.querySelectorAll('.circle');
    var gradeSelect = document.querySelector('#gradeSelect');
    var submitButton = document.querySelector('#submitButton');

    submitButton.disabled = true;

    circles.forEach(function (circle) {
        circle.addEventListener('click', function () {
            circles.forEach(function (c) {
                c.classList.remove('button-create-user');
                c.classList.add('button-create-test');
            });

            circle.classList.remove('button-create-test');
            circle.classList.add('button-create-user');
            gradeSelect.value = circle.getAttribute('data-value');

            submitButton.disabled = false;
        });
    });
});