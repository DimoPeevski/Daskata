// Fetch and preview an exam based on ExamUrl
function previewExam(examUrl) {
    window.location.href = '/Exam/Preview/' + examUrl;
}

// Fetch and preview an exam based on ExamUrl in user profile
function previewExamInProfile(examUrl) {
    window.open(examUrl, '_self');

}