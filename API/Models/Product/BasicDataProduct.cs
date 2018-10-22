namespace API.Models.Product
{
    /// <summary>
    /// Represents what a product is.
    /// </summary>
    public class BasicDataProduct
    {
        /// <summary>
        /// Product id.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Product (unique) name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Product price ; must not be negative.
        /// </summary>
        public int Price { get; set; }
    }
}
