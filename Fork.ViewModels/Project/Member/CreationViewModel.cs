using Fork.ViewModels.User;

namespace Fork.ViewModels.Project.Member
{
    public class CreationViewModel
    {
        /// <summary>
        /// Project id where to add member.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }
    }
}
