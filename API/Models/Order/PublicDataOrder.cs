using System;

namespace API.Models.Order
{
    /// <summary>
    /// Represents what an order is.
    /// </summary>
    public class PublicDataOrder
    {
        /// <summary>
        /// Order id.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Client id, who is a user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Order creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
