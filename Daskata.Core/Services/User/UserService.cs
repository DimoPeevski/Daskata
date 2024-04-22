using Daskata.Core.Contracts.User;
using Daskata.Infrastructure.Common;

namespace Daskata.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }
    }
}
