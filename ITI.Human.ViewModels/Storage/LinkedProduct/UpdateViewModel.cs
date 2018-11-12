namespace ITI.Human.ViewModels.Storage.LinkedProduct
{
    public class UpdateViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataStorageLinkedProduct.StorageLinkedProductId"/>.
        /// </summary>
        public int StorageLinkedProductId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageLinkedProduct.UnitPrice"/>.
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageLinkedProduct.Stock"/>.
        /// </summary>
        public int Stock { get; set; }
    }
}
