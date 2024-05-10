using Daskata.Core.Services.User;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Daskata.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<UserProfile>> _mockUserManager;
        private readonly Mock<SignInManager<UserProfile>> _mockSignInManager;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IRepository> _mockRepository;

        public UserServiceTests()
        {
            _mockUserManager = new Mock<UserManager<UserProfile>>();
            _mockSignInManager = new Mock<SignInManager<UserProfile>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockRepository = new Mock<IRepository>();
        }

        [Test]
        public async Task LoginUserAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var userService = new UserService(_mockUserManager.Object, _mockSignInManager.Object, _mockHttpContextAccessor.Object, _mockRepository.Object);
            string userName = "testUser";
            string password = "testPassword";

            // Act
            var result = await userService.LoginUserAsync(userName, password);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task LogoutUserAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            var userService = new UserService(_mockUserManager.Object, _mockSignInManager.Object, _mockHttpContextAccessor.Object, _mockRepository.Object);

            // Act
            await userService.LogoutUserAsync();

            // Assert
            // Ако хвърли грешка значи е успешен.
        }

        [Test]
        public async Task RegisterUserAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var userService = new UserService(_mockUserManager.Object, _mockSignInManager.Object, _mockHttpContextAccessor.Object, _mockRepository.Object);
            var model = new RegisterUserFormModel();
            Guid createdByUserId = Guid.NewGuid();

            // Act
            var result = await userService.RegisterUserAsync(model, createdByUserId);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetCurentUserId_ShouldReturnExpectedResult()
        {
            // Arrange
            var userService = new UserService(_mockUserManager.Object, _mockSignInManager.Object, _mockHttpContextAccessor.Object, _mockRepository.Object);

            // Act
            var result = await userService.GetCurentUserId();

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GenerateUniqueUsernameAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var userService = new UserService(_mockUserManager.Object, _mockSignInManager.Object, _mockHttpContextAccessor.Object, _mockRepository.Object);

            // Act
            var result = await userService.GenerateUniqueUsernameAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}
