namespace Fork.ViewModels.User
{
    /// <summary>
    /// Defines what a User Balance is.
    /// </summary>
    public class BasicDataUserBalance
    {
        /// <summary>
        /// Current User's balance.
        /// </summary>
        public int UserBalanceId { get; set; }

        /// <summary>
        /// Current project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataUser.UserName"/>.
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

        /// <summary>
        /// See <see cref="DetailedDataUserReferenceTooltip.AvatarUrl"/>.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Current User's balance.
        /// </summary>
        public int Balance { get; set; }
    }
}
