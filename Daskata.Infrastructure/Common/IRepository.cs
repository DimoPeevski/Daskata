namespace Daskata.Infrastructure.Common
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;

        IQueryable<T> AllReadonly<T>() where T : class;
    }
}
