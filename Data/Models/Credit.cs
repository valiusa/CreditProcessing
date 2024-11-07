using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Credit
    {
        private long DateOfCreditApplication; // Store as Unix timestamp in the database

        public int Id { get; set; }
        public int CreditNumber { get; set; }
        public string ClientName { get; set; }
        public decimal CreditAmount { get; set; }
        public DateTime DateOfCreaditApplication
        {
            get => DateTimeOffset.FromUnixTimeSeconds(DateOfCreditApplication).DateTime;
            set => DateOfCreditApplication = new DateTimeOffset(value).ToUnixTimeSeconds();
        }
        public int CreditStatus { get; set; }

        public List<Invoice> Invoices { get; set; }
    }
}
