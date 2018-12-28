using System;
using System.Drawing;

namespace ITI.Human.ViewModels.User
{
    /// <summary>
    /// Defines what a School Member is.
    /// </summary>
    public class DetailedDataUserReferenceTooltip
    {
        /// <summary>
        /// See <see cref="BasicDataUser.UserId"/>
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataUser.UserName"/>
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's avatar URL.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// User's FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's BirthDate.
        /// </summary>
        public DateTime BirthDate { get; set; }

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
