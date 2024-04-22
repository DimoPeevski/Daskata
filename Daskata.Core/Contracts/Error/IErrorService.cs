namespace Daskata.Core.Contracts
{
    public interface IErrorService
    {
        Task LogAccessDeniedAsync(Guid? userId);
    }
}
