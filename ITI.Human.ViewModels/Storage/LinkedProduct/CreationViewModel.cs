namespace ITI.Human.ViewModels.Storage.LinkedProduct
{
    public class CreationViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataStorageLinkedProduct.StorageId"/>.
        /// </summary>
        public int StorageId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageLinkedProduct.ProductId"/>.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageLinkedProduct.UnitPrice"/>.
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageLinkedProduct.Stock"/>.
        /// </summary>
        public int Stock { get; set; }
    }
}
