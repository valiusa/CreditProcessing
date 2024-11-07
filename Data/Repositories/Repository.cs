using Dapper;
using Data.Entities;
using Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection;
        protected readonly string _tableName;

        public Repository(IDbConnection dbConnection, string tableName)
        {
            _dbConnection = dbConnection;
            _tableName = tableName;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<T>(
                $"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<T>($"SELECT * FROM {_tableName}");
        }

        public async Task AddAsync(T entity)
        {
            var insertSql = GenerateInsertSql(entity);
            await _dbConnection.ExecuteAsync(insertSql, entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var updateSql = GenerateUpdateSql(entity);
            await _dbConnection.ExecuteAsync(updateSql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _dbConnection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id = @Id", new { Id = id });
        }

        private string GenerateInsertSql(T entity)
        {
            if (entity is Credit)
            {
                return "INSERT INTO Credits (CreditNumber, ClientName, CreditAmount, DateOfCreditApplication, CreditStatus) VALUES (@CreditNumber, @ClientName, @CreditAmount, @DateOfCreditApplication, @CreditStatus)";
            }

            if (entity is Invoice)
            {
                return "INSERT INTO Invoices (InvoiceNumber, CreditId, InvoiceAmount) VALUES (@InvoiceNumber, @CreditId, @InvoiceAmount)";
            }

            throw new NotSupportedException($"Entity type {typeof(T)} is not supported.");
        }

        private string GenerateUpdateSql(T entity)
        {
            if (entity is Credit)
            {
                return "UPDATE Credits SET CreditNumber = @CreditNumber, ClientName = @ClientName, CreditAmount = @CreditAmount, DateOfCreditApplication = @DateOfCreditApplication, CreditStatus = @CreditStatus WHERE Id = @Id";
            }

            if (entity is Invoice)
            {
                return "UPDATE Invoices SET InvoiceNumber = @InvoiceNumber, CreditId = @CreditId, InvoiceAmount = @InvoiceAmount WHERE Id = @Id";
            }

            throw new NotSupportedException($"Entity type {typeof(T)} is not supported.");
        }
    }
}
