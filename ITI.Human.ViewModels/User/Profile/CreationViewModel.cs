namespace ITI.Human.ViewModels.User.Profile
{
    public class CreationViewModel
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataUserReferenceTooltip.FirstName"/>.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// See <see cref="DetailedDataUserReferenceTooltip.LastName"/>.
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// User's biography.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// User's current school status.
        /// </summary>
        public int SchoolStatusId { get; set; }

        /// <summary>
        /// Necessary secret code for specific status.
        /// </summary>
        public string SecretCode { get; set; }

        /// <summary>
        /// Student's current semester (> 0 if student).
        /// </summary>
        public int SemesterId { get; set; }
    }
}
