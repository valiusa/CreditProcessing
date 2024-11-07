using Dapper;
using Data.Entities;
using Data.Models;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CreditRepository : Repository<Credit>, ICreditRepository
    {
        public CreditRepository(IDbConnection dbConnection) : base (dbConnection, "Credits")
        {
        }

        public async Task<IEnumerable<Credit>> GetAllWithRelationsAsync(Enums.CreditRelations relations)
        {
            var sql = new StringBuilder($"SELECT * FROM {_tableName}");

            // Check if any relations are specified
            if (relations.HasFlag(Enums.CreditRelations.Invoices))
            {
                sql.Append(" LEFT JOIN Invoices ON Invoices.CreditId = Credits.Id");
            }

            var query = sql.ToString();

            var creditDictionary = new Dictionary<int, Credit>();

            await _dbConnection.QueryAsync<Credit, Invoice, Credit>(
                query,
                (credit, invoice) =>
                {
                    if (!creditDictionary.ContainsKey(credit.Id))
                    {
                        credit.Invoices = new List<Invoice>();
                        creditDictionary.Add(credit.Id, credit);
                    }

                    if (invoice != null)
                    {
                        creditDictionary[credit.Id].Invoices.Add(invoice);
                    }

                    return credit;
                },
                splitOn: "Id"
            );

            return creditDictionary.Values;
        }

        public async Task<CreditSummary> GetCreditsSummaryAsync()
        {
            var summary = new CreditSummary();

            var sqlSum = @$"
                    SELECT 
                        SUM(CreditAmount) FROM {_tableName} WHERE CreditStatus = @StatusPaid;
                    SELECT 
                        SUM(CreditAmount) FROM {_tableName} WHERE CreditStatus = @StatusAwaitingPayment;";

            // To fetch the sums in one go
            using (var multi = await _dbConnection.QueryMultipleAsync(sqlSum, new
            {
                StatusPaid = (int)Enums.CreditStatuses.Paid,
                StatusAwaitingPayment = (int)Enums.CreditStatuses.AwaitingPayment
            }))
            {
                summary.CreditsAmountWithStatusPaid = await multi.ReadSingleOrDefaultAsync<decimal>();
                summary.CreditsAmountWithStatusAwaitingPayment = await multi.ReadSingleOrDefaultAsync<decimal>();
            }

            return summary;
        }
    }
}
