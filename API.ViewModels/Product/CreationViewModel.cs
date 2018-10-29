namespace API.ViewModels.Product
{
    public class CreationViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataProduct.Name"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Desc"/>.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Price"/>.
        /// </summary>
        public double Price { get; set; }
    }
}
