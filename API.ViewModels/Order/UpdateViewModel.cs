using API.ViewModels.Product.Ordered;
using System.Collections.Generic;

namespace API.ViewModels.Order
{
    public class UpdateViewModel
    {
        /// <summary>
        /// See <see cref="DetailedDataOrder.OrderInfo"/>.
        /// </summary>
        public BasicDataOrder Info { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataOrder.Products"/>.
        /// </summary>
        public IEnumerable<BasicDataOrderedProduct> Products { get; set; }
    }
}
