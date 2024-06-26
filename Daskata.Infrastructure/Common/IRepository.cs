﻿namespace Daskata.Infrastructure.Common
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;

        IQueryable<T> AllReadonly<T>() where T : class;

        Task AddAsync<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;

        Task<int> SaveChangesAsync();

        
    }
}
