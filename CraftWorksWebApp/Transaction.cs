using System;
namespace CraftWorksWebApp
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
