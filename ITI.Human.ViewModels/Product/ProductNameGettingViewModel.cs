namespace ITI.Human.ViewModels.Product
{
    /// <summary>
    /// Assuming a Product name can contain spaces,
    /// one prefer use an appropriate ViewModel.
    /// </summary>
    public class ProductNameGettingViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataProduct.Name"/>.
        /// </summary>
        public string Name { get; set; }
    }
}
