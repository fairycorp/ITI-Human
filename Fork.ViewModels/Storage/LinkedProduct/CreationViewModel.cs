using Fork.ViewModels.User;

namespace Fork.ViewModels.Storage.LinkedProduct
{
    public class CreationViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageSLP.StorageId"/>.
        /// </summary>
        public int StorageId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageSLP.ProductId"/>.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageSLP.UnitPrice"/>.
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageSLP.Stock"/>.
        /// </summary>
        public int Stock { get; set; }
    }
}
