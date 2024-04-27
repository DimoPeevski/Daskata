using Daskata.Core.Services.Exam;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Moq;

[TestFixture]
public class ExamServiceTests
{
    private Mock<IRepository> _repositoryMock;
    private ExamService _examService;
    private readonly Guid _mockUserId = Guid.NewGuid();


    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository>();
    }

    [Test]
    public async Task GetAllExamsAsync_WithExams_ReturnsNonEmptyList()
    {
        // Arrange
        var mockExams = new List<Exam> { new Exam
        {

        }
        };
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(mockExams.AsQueryable());

        // Act
        var result = await _examService.GetAllExamsAsync();

        // Assert
        Assert.IsNotEmpty(result);
    }

    [Test]
    public async Task GetAllExamsAsync_WhenNoExams_ReturnsEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(Enumerable.Empty<Exam>().AsQueryable());

        // Act
        var result = await _examService.GetAllExamsAsync();

        // Assert
        Assert.IsEmpty(result);
    }

    [Test]
    public void GetAllExamsAsync_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.All<Exam>()).Throws(new Exception());

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _examService.GetAllExamsAsync());
    }

    [Test]
    public async Task GetAllExamsAsync_CallsRepositoryAllMethodOnce()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(new List<Exam>().AsQueryable());

        // Act
        await _examService.GetAllExamsAsync();

        // Assert
        _repositoryMock.Verify(repo => repo.All<Exam>(), Times.Once());
    }

    [Test]
    public async Task GetAllExamsAsync_MapsPropertiesCorrectly()
    {
        // Arrange
        var mockExam = new Exam
        {
            Id = Guid.NewGuid(),
            Title = "Hello",
            Description = "Test",
            TotalPoints = 20,
            IsPublic = false
        };
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(new List<Exam> { mockExam }.AsQueryable());

        // Act
        var result = await _examService.GetAllExamsAsync();
        var examViewModel = result.First();

        // Assert
        Assert.AreEqual(mockExam.Id, examViewModel.Id);
        Assert.AreEqual(mockExam.Title, examViewModel.Title);
        Assert.AreEqual(mockExam.Description, examViewModel.Description);
        Assert.AreEqual(mockExam.TotalPoints, examViewModel.TotalPoints);
        Assert.AreEqual(mockExam.IsPublic, examViewModel.IsPublic);
    }

    [Test]
    public async Task GetExamsByCreatorAsync_WithExams_ReturnsNonEmptyList()
    {
        // Arrange
        var mockExams = new List<Exam> { new Exam { CreatedByUserId = _mockUserId /*, other properties */ } };
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(mockExams.AsQueryable());

        // Act
        var result = await _examService.GetExamsByCreatorAsync(_mockUserId);

        // Assert
        Assert.IsNotEmpty(result);
        Assert.IsTrue(result.All(e => e.CreatedByUserId == _mockUserId));
    }

    [Test]
    public async Task GetExamsByCreatorAsync_WhenNoExams_ReturnsEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(new List<Exam>().AsQueryable());

        // Act
        var result = await _examService.GetExamsByCreatorAsync(_mockUserId);

        // Assert
        Assert.IsEmpty(result);
    }

    [Test]
    public async Task GetExamsByCreatorAsync_FiltersByUserId()
    {
        // Arrange
        var mockExams = new List<Exam>
        {
            new Exam { CreatedByUserId = _mockUserId },
            new Exam { CreatedByUserId = Guid.NewGuid() }
        };
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(mockExams.AsQueryable());

        // Act
        var result = await _examService.GetExamsByCreatorAsync(_mockUserId);

        // Assert
        Assert.IsTrue(result.All(e => e.CreatedByUserId == _mockUserId) && result.Count == 1);
    }

    [Test]
    public async Task GetExamsByCreatorAsync_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.All<Exam>()).Throws(new Exception());

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _examService.GetExamsByCreatorAsync(_mockUserId));
    }

    [Test]
    public async Task GetExamsByCreatorAsync_CallsRepositoryAllMethodOnce()
    {
        // Arrange
        var mockExams = new List<Exam> { new Exam { CreatedByUserId = _mockUserId } };
        _repositoryMock.Setup(repo => repo.All<Exam>()).Returns(mockExams.AsQueryable());

        // Act
        await _examService.GetExamsByCreatorAsync(_mockUserId);

        // Assert
        _repositoryMock.Verify(repo => repo.All<Exam>(), Times.Once());
    }
}
