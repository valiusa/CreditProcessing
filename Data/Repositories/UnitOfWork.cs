using Data.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;

        public ICreditRepository Credits { get; }

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            Credits = new CreditRepository(_dbConnection);
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
