// Exam confirmation box just before the exam pass
document.addEventListener('DOMContentLoaded', function () {
    var startButton = document.getElementById('startBtn');
    var backButton = document.getElementById('backBtn');
    var confirmBlock = document.getElementById('confirmBlock');
    var examBody = document.getElementById('examBody');

    examBody.classList.add('blurry');

    startButton.addEventListener('click', function () {
        confirmBlock.style.display = 'none';
        examBody.classList.remove('blurry');
    });

    backButton.addEventListener('click', function () {
        window.history.back();
    });
});