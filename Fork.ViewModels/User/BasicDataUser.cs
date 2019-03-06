using System;

namespace Fork.ViewModels.User
{
    public class BasicDataUser
    {
        /// <summary>
        /// User's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  User's name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's account creation date.
        /// </summary>
        public DateTime Creationdate { get; set; }
    }
}
