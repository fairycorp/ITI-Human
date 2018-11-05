using API.ViewModels.Product.Ordered;
using System.Collections.Generic;

namespace API.ViewModels.Order
{
    public class CreationViewModel
    {
        /// <summary>
        /// User's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataOrder.ClassroomName"/> (this is the id version of it).
        /// </summary>
        public int ClassroomId { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataOrder.Products"/>.
        /// </summary>
        public IEnumerable<BasicDataProductToOrder> Products { get; set; }
    }
}
