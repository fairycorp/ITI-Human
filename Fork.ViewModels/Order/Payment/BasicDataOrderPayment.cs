using System;

namespace Fork.ViewModels.Order.Payment
{
    public class BasicDataOrderPayment
    {
        /// <summary>
        /// Current Order Payment id.
        /// </summary>
        public int OrderPaymentId { get; set; }

        /// <summary>
        /// Current Order Final Due id.
        /// </summary>
        public int OrderFinalDueId { get; set; }

        /// <summary>
        /// Current Ordered Product id.
        /// </summary>
        public int OrderedProductId { get; set; }

        /// <summary>
        /// Paid amount.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// When has the payment been made ?
        /// </summary>
        public DateTime PaymentTime { get; set; }
    }
}
