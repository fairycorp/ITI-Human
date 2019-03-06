using Fork.ViewModels.User;

namespace Fork.ViewModels.Project.Member
{
    /// <summary>
    /// Defines what a Project Member is.
    /// </summary>
    public class DetailedDataProjectMember
    {
        /// <summary>
        /// Current project member's id.
        /// </summary>
        public int ProjectMemberId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProject.ProjectId"/>.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProject.ProjectName"/>.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Current project member's rank id.
        /// </summary>
        public int ProjectRankId { get; set; }

        /// <summary>
        /// Current project member's rank name.
        /// </summary>
        public string ProjectRankName { get; set; }

        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataUser.UserName"/>.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's avatar url.
        /// </summary>
        public string AvatarUrl { get; set; }
    }
}
