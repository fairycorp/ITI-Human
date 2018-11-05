namespace API.ViewModels.Product.Ordered
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
        /// Matching product id.
        /// </summary>
        public int ProductId { get; set; }
        
        /// <summary>
        /// See <see cref="BasicDataProduct.Name"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Desc"/>
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Price"/>.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProductToOrder.Amount"/>.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Has the product been delivered ?
        /// </summary>
        public bool HasBeenDelivered { get; set; }
    }
}
