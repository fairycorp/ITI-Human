using Fork.ViewModels.Product.Ordered;
using System.Collections.Generic;

namespace Fork.ViewModels.Order
{
    /// <summary>
    /// A Detailed Order is composed of a <see cref="BasicDataOrder"/> and a list of <see cref="DetailedDataOrderedProduct"/>.
    /// </summary>
    public class DetailedDataOrder
    {
        /// <summary>
        /// Information on the mentionned Order.
        /// </summary>
        public BasicDataOrder Info { get; set; }

        /// <summary>
        /// Products that mentionned Order contains.
        /// </summary>
        public IEnumerable<DetailedDataOrderedProduct> Products { get; set; }
    }
}
