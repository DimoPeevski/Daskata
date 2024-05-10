using NUnit.Framework;
using Moq;
using Daskata.Core.Contracts.Exam;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestFixture]
public class ExamServiceTests
{
    private Mock<IExamService> _mockExamService;
    private Guid _userId;
    private string _examUrl, _subject, _grade;
    private FullExamViewModel _fullExamViewModel;
    private ExamAttemptViewModel _examAttemptViewModel;
    private List<QuestionViewModel> _questions;
    private int _maxPossiblePoints;

    [SetUp]
    public void Setup()
    {
        _mockExamService = new Mock<IExamService>();
        _userId = Guid.NewGuid();
        _examUrl = "TestExamUrl";
        _subject = "TestSubject";
        _grade = "TestGrade";
        _fullExamViewModel = new FullExamViewModel();
        _examAttemptViewModel = new ExamAttemptViewModel();
        _questions = new List<QuestionViewModel>();
        _maxPossiblePoints = 100;
    }

    [Test]
    public async Task GetExamsByCreatorAsync_ShouldReturnListOfFullExamViewModel()
    {
        var expectedModel = new List<FullExamViewModel>();
        _mockExamService.Setup(service => service.GetExamsByCreatorAsync(_userId))
            .ReturnsAsync(expectedModel);

        var result = await _mockExamService.Object.GetExamsByCreatorAsync(_userId);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task GetExamPreview_ShouldReturnFullExamViewModel()
    {
        _mockExamService.Setup(service => service.GetExamPreview(_examUrl))
            .ReturnsAsync(_fullExamViewModel);

        var result = await _mockExamService.Object.GetExamPreview(_examUrl);

        Assert.AreEqual(_fullExamViewModel, result);
    }

    [Test]
    public async Task GetOpenExam_ShouldReturnFullExamViewModel()
    {
        _mockExamService.Setup(service => service.GetOpenExam(_examUrl))
            .ReturnsAsync(_fullExamViewModel);

        var result = await _mockExamService.Object.GetOpenExam(_examUrl);

        Assert.AreEqual(_fullExamViewModel, result);
    }

    [Test]
    public async Task Create_ShouldReturnListOfString()
    {
        var expectedModel = new List<string>();
        _mockExamService.Setup(service => service.Create())
            .ReturnsAsync(expectedModel);

        var result = await _mockExamService.Object.Create();

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task Grade_ShouldReturnListOfString()
    {
        var expectedModel = new List<string>();
        _mockExamService.Setup(service => service.Grade(_subject))
            .ReturnsAsync(expectedModel);

        var result = await _mockExamService.Object.Grade(_subject);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task Details_ShouldReturnFullExamViewModel()
    {
        _mockExamService.Setup(service => service.Details(_grade))
            .ReturnsAsync(_fullExamViewModel);

        var result = await _mockExamService.Object.Details(_grade);

        Assert.AreEqual(_fullExamViewModel, result);
    }

    [Test]
    public async Task Publish_ShouldReturnExam()
    {
        var expectedModel = new Exam();
        _mockExamService.Setup(service => service.Publish(_fullExamViewModel, _subject, _grade, _userId))
            .ReturnsAsync(expectedModel);

        var result = await _mockExamService.Object.Publish(_fullExamViewModel, _subject, _grade, _userId);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task GetExamByUrlAsync_ShouldReturnExam()
    {
        var expectedModel = new Exam();
        _mockExamService.Setup(service => service.GetExamByUrlAsync(_examUrl))
            .ReturnsAsync(expectedModel);

        var result = await _mockExamService.Object.GetExamByUrlAsync(_examUrl);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task UpdateExamAsync_ShouldReturnExam()
    {
        var expectedModel = new Exam();
        _mockExamService.Setup(service => service.UpdateExamAsync(_fullExamViewModel))
            .ReturnsAsync(expectedModel);

        var result = await _mockExamService.Object.UpdateExamAsync(_fullExamViewModel);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task GetExamAndQuestionsByUrlAsync_ShouldReturnFullExamViewModel()
    {
        _mockExamService.Setup(service => service.GetExamAndQuestionsByUrlAsync(_examUrl))
            .ReturnsAsync(_fullExamViewModel);

        var result = await _mockExamService.Object.GetExamAndQuestionsByUrlAsync(_examUrl);

        Assert.AreEqual(_fullExamViewModel, result);
    }

    [Test]
    public async Task CalculateExamResultAsync_ShouldReturnExamAttempt()
    {
        var expectedModel = new ExamAttempt();
        _mockExamService.Setup(service => service.CalculateExamResultAsync(_examUrl, _examAttemptViewModel))
            .ReturnsAsync(expectedModel);

        var result = await _mockExamService.Object.CalculateExamResultAsync(_examUrl, _examAttemptViewModel);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public void CalculateTotalScoreInPercentage_ShouldReturnDouble()
    {
        var expectedModel = 0.0;
        _mockExamService.Setup(service => service.CalculateTotalScoreInPercentage(_questions, _maxPossiblePoints))
            .Returns(expectedModel);

        var result = _mockExamService.Object.CalculateTotalScoreInPercentage(_questions, _maxPossiblePoints);

        Assert.AreEqual(expectedModel, result);
    }
}
