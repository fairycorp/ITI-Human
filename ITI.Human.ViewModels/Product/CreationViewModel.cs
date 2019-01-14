using ITI.Human.ViewModels.User;

namespace ITI.Human.ViewModels.Product
{
    public class CreationViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Name"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Desc"/>.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Url"/>.
        /// </summary>
        public string Url { get; set; }
    }
}
