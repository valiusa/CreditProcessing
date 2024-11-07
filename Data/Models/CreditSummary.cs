using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreditSummary
    {
        public decimal CreditsAmountWithStatusPaid { get; set; }
        public decimal CreditsAmountWithStatusAwaitingPayment { get; set; }

        private decimal GetTotal => this.CreditsAmountWithStatusPaid + this.CreditsAmountWithStatusAwaitingPayment;

        public string PercentagePaid => GetPercentage(CreditsAmountWithStatusPaid);

        public string PercentageAwaitingPayment => GetPercentage(CreditsAmountWithStatusAwaitingPayment);

        private string GetPercentage(decimal amount)
        {
            return GetTotal > 0
                ? ((amount / GetTotal) * 100).ToString("F2") + "%"
                : "0.00%"; // Handle division by zero
        }
    }
}
