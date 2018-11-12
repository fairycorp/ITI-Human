namespace ITI.Human.ViewModels.Product.Ordered
{
    public class BasicDataProductToOrder
    {
        /// <summary>
        /// See <see cref="BasicDataOrderedProduct.StorageLinkedProductId"/>.
        /// </summary>
        public int StorageLinkedProductId { get; set; }

        /// <summary>
        /// Desired Product quantity.
        /// </summary>
        public int Quantity { get; set; }
    }
}
