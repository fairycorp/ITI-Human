using System;

namespace ITI.Human.ViewModels.Order
{
    /// <summary>
    /// Defines what an Order Credit is.
    /// </summary>
    public class BasicDataOrderCredit
    {
        /// <summary>
        /// Current Order Credit id.
        /// </summary>
        public int OrderCreditId { get; set; }

        /// <summary>
        /// Current Project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Current User's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Credit amount.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Credit date.
        /// </summary>
        public DateTime CreditTime { get; set; }
    }
}