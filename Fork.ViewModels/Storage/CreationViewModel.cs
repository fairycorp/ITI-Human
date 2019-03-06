using Fork.ViewModels.User;

namespace Fork.ViewModels.Storage
{
    public class CreationViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorage.ProjectId"/>.
        /// </summary>
        public int ProjectId { get; set; }
    }
}
