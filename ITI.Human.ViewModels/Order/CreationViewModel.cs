using ITI.Human.ViewModels.Product.Ordered;
using System.Collections.Generic;

namespace ITI.Human.ViewModels.Order
{
    public class CreationViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataOrder.StorageId"/>.
        /// </summary>
        public int StorageId { get; set; }

        /// <summary>
        /// User's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataOrder.ClassroomId"/>.
        /// </summary>
        public int ClassroomId { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataOrder.Products"/>.
        /// </summary>
        public IEnumerable<BasicDataProductToOrder> Products { get; set; }
    }
}
