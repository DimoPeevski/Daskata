namespace Daskata.Core.Services.Error
{
    public interface IErrorService
    {
        Task LogAccessDeniedAsync(Guid? userId);
    }
}
