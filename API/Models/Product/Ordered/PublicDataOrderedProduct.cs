namespace API.Models.Product.Ordered
{
    /// <summary>
    /// Represents what an ordered product is.
    /// </summary>
    public class PublicDataOrderedProduct
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
    }
}
