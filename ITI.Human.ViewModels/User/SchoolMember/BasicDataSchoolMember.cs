namespace ITI.Human.ViewModels.User.SchoolMember
{
    /// <summary>
    /// Defines what a School Member is.
    /// </summary>
    public class BasicDataUserReferenceTooltip
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataUser.UserName"/>
        /// </summary>
        public int UserName { get; set; }

        /// <summary>
        /// Current School Member's id.
        /// </summary>
        public int SchoolMemberId { get; set; }

        /// <summary>
        /// Current School Member's status id.
        /// </summary>
        public int SchoolStatusId { get; set; }

        /// <summary>
        /// Current School Member's status name.
        /// </summary>
        public string SchoolStatusName { get; set; }
    }
}
