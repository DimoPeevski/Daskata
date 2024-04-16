//Check the question type and if true/false type - locks the 2nd option
document.addEventListener("DOMContentLoaded", function (event) {
    var questionTypeSelect = document.getElementById('QuestionType');
    var isMultipleCorrectSelect = document.getElementById('IsMultipleCorrect');

    function adjustIsMultipleCorrect() {
        var questionType = questionTypeSelect.options[questionTypeSelect.selectedIndex].value;
        if (questionType === 'TrueFalse') {
            isMultipleCorrectSelect.value = 'false';
            isMultipleCorrectSelect.disabled = true;
        } else {
            isMultipleCorrectSelect.disabled = false;
        }
    }

    questionTypeSelect.addEventListener('change', adjustIsMultipleCorrect);

    adjustIsMultipleCorrect();
});
