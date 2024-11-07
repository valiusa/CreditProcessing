using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public int CreditId { get; set; }
        public decimal InvoiceAmount { get; set; }
        //public Credit Credit { get; set; }
    }
}
