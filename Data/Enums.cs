using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Enums
    {
        public enum CreditStatuses : int
        {
            Created = 1,
            AwaitingPayment = 2,
            Paid = 3
        }

        [Flags]
        public enum CreditRelations : int
        {
            None = 0,
            Invoices = 1
        }
    }
}
