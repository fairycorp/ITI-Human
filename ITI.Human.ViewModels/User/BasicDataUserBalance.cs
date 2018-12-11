namespace ITI.Human.ViewModels.User
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
        /// Current User's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Current balance.
        /// </summary>
        public int Balance { get; set; }
    }
}
