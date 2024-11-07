using Data.Entities;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface ICreditRepository
    {
        Task<IEnumerable<Credit>> GetAllWithRelationsAsync(Enums.CreditRelations relations);
        Task<CreditSummary> GetCreditsSummaryAsync();
    }
}
