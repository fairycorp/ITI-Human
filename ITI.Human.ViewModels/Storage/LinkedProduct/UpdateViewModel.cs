using ITI.Human.ViewModels.User;

namespace ITI.Human.ViewModels.Storage.LinkedProduct
{
    public class UpdateViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageSLP.StorageLinkedProductId"/>.
        /// </summary>
        public int StorageLinkedProductId { get; set; }

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
