namespace ITI.Human.ViewModels.User.Profile
{
    public class BasicDataUserProfile
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataUserReferenceTooltip.UserName"/>.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataUserReferenceTooltip.FirstName"/>.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataUserReferenceTooltip.LastName"/>.
        /// </summary>
        public string LastName { get; set; }
    }
}
