namespace API.ViewModels.Product.Ordered
{
    public class BasicDataProductToOrder
    {
        /// <summary>
        /// See <see cref="BasicDataProduct.ProductId"/>.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Desired Product amount.
        /// </summary>
        public int Amount { get; set; }
    }
}
