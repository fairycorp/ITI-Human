namespace API.Models.Product
{
    /// <summary>
    /// Assuming a Product name can contain spaces,
    /// one prefer use an appropriate ViewModel.
    /// </summary>
    public class ProductGettingViewModel
    {
        /// <summary>
        /// See <see cref="PublicDataProduct.Name"/>.
        /// </summary>
        public string Name { get; set; }
    }
}
