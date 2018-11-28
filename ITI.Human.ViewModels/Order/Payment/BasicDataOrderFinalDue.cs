namespace ITI.Human.ViewModels.Order.Payment
{
    public class BasicDataOrderFinalDue
    {
        /// <summary>
        /// Current Order Final Due id.
        /// </summary>
        public int OrderFinalDueId { get; set; }

        /// <summary>
        /// Current Order id.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Total amount to pay.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Already paid amount.
        /// </summary>
        public int Paid { get; set; }
    }
}
