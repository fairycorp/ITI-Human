using API.Models.Product.Ordered;
using System.Collections.Generic;

namespace API.Models.Order
{
    /// <summary>
    /// Represents what a detailed Order is.
    /// </summary>
    public class DetailedDataOrder
    {
        /// <summary>
        /// Information on the mentionned Order.
        /// </summary>
        public BasicDataOrder OrderInfo { get; set; }

        /// <summary>
        /// Products that mentionned Order contains.
        /// </summary>
        public List<BasicDataOrderedProduct> Products { get; set; }
    }
}
