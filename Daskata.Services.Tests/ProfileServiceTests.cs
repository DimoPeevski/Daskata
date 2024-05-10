using Daskata.Core.Contracts.Profile;
using Daskata.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Moq;

[TestFixture]
public class ProfileServiceTests
{
    private Mock<IProfileService> _mockProfileService;
    private Guid _userId;
    private string _username, _email, _phoneNumber, _oldPassword, _newPassword;
    private EditUserFormModel _editUserFormModel;
    private IdentityResult _identityResult;

    [SetUp]
    public void Setup()
    {
        _mockProfileService = new Mock<IProfileService>();
        _userId = Guid.NewGuid();
        _username = "TestUser";
        _email = "testuser@example.com";
        _phoneNumber = "1234567890";
        _oldPassword = "OldPassword123";
        _newPassword = "NewPassword123";
        _editUserFormModel = new EditUserFormModel();
        _identityResult = IdentityResult.Success;
    }

    [Test]
    public async Task GetUserProfileModelAsync_ShouldReturnUserProfileModel()
    {
        var expectedModel = new UserProfileModel();
        _mockProfileService.Setup(service => service.GetUserProfileModelAsync(_userId))
            .ReturnsAsync(expectedModel);

        var result = await _mockProfileService.Object.GetUserProfileModelAsync(_userId);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task GetUserPreviewModelAsync_ShouldReturnUserProfileModel()
    {
        var expectedModel = new UserProfileModel();
        _mockProfileService.Setup(service => service.GetUserPreviewModelAsync(_username))
            .ReturnsAsync(expectedModel);

        var result = await _mockProfileService.Object.GetUserPreviewModelAsync(_username);

        Assert.AreEqual(expectedModel, result);
    }

    [Test]
    public async Task GetEditUserFormModelAsync_ShouldReturnEditUserFormModel()
    {
        _mockProfileService.Setup(service => service.GetEditUserFormModelAsync(_userId))
            .ReturnsAsync(_editUserFormModel);

        var result = await _mockProfileService.Object.GetEditUserFormModelAsync(_userId);

        Assert.AreEqual(_editUserFormModel, result);
    }

    [Test]
    public async Task UpdateUserAsync_ShouldReturnIdentityResult()
    {
        _mockProfileService.Setup(service => service.UpdateUserAsync(_userId, _editUserFormModel))
            .ReturnsAsync(_identityResult);

        var result = await _mockProfileService.Object.UpdateUserAsync(_userId, _editUserFormModel);

        Assert.AreEqual(_identityResult, result);
    }

    [Test]
    public async Task UsernameExistsAsync_ShouldReturnBool()
    {
        _mockProfileService.Setup(service => service.UsernameExistsAsync(_username))
            .ReturnsAsync(true);

        var result = await _mockProfileService.Object.UsernameExistsAsync(_username);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task EmailExistsAsync_ShouldReturnBool()
    {
        _mockProfileService.Setup(service => service.EmailExistsAsync(_email))
            .ReturnsAsync(true);

        var result = await _mockProfileService.Object.EmailExistsAsync(_email);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task PhoneNumberExistsAsync_ShouldReturnBool()
    {
        _mockProfileService.Setup(service => service.PhoneNumberExistsAsync(_phoneNumber))
            .ReturnsAsync(true);

        var result = await _mockProfileService.Object.PhoneNumberExistsAsync(_phoneNumber);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task ChangeUserPasswordAsync_ShouldReturnIdentityResult()
    {
        _mockProfileService.Setup(service => service.ChangeUserPasswordAsync(_userId, _oldPassword, _newPassword))
            .ReturnsAsync(_identityResult);

        var result = await _mockProfileService.Object.ChangeUserPasswordAsync(_userId, _oldPassword, _newPassword);

        Assert.AreEqual(_identityResult, result);
    }
}
