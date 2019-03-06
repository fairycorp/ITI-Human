using System.Collections.Generic;

namespace Fork.ViewModels.Order.Payment
{
    /// <summary>
    /// Represents what a detailed Order Final due is.
    /// </summary>
    public class DetailedDataOrderFinalDue
    {
        /// <summary>
        /// Information of the mentionned Order Final Due
        /// </summary>
        public BasicDataOrderFinalDue Info { get; set; }

        /// <summary>
        /// List of its linked payments.
        /// </summary>
        public IEnumerable<BasicDataOrderPayment> Payments { get; set; }
    }
}
