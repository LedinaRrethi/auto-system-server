﻿using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace DAL.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class;
        int Save();

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<int> CommitAsync();

    }
}