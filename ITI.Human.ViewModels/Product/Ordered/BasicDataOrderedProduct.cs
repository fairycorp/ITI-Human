using ITI.Human.ViewModels.Order;

namespace ITI.Human.ViewModels.Product.Ordered
{
    /// <summary>
    /// Represents what an ordered product is.
    /// </summary>
    public class BasicDataOrderedProduct
    {
        /// <summary>
        /// Ordered product id.
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
        /// Storage linked product unit price.
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProductToOrder.Quantity"/>.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Ordered product current state.
        /// </summary>
        public State CurrentState { get; set; }
    }
}
