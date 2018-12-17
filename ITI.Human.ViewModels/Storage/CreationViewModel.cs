using ITI.Human.ViewModels.User;

namespace ITI.Human.ViewModels.Storage
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
