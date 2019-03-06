using Fork.ViewModels.Order;

namespace Fork.ViewModels.Product.Ordered
{
    public class BasicDataOrderedProduct
    {
        /// <summary>
        /// Ordered Product id.
        /// </summary>
        public int OrderedProductId { get; set; }

        /// <summary>
        /// Matching order id.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Matching storage linked product id.
        /// </summary>
        public int StorageLinkedProductId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Name"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Desc"/>
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageSLP.UnitPrice"/>.
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProductToOrder.Quantity"/>.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Ordered Product current state.
        /// </summary>
        public State CurrentState { get; set; }

        /// <summary>
        /// Ordered Product payment state.
        /// </summary>
        public Payment PaymentState { get; set; }
    }
}
