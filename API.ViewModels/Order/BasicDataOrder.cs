using System;

namespace API.ViewModels.Order
{
    /// <summary>
    /// Represents what an order is.
    /// </summary>
    public class BasicDataOrder
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

        /// <summary>
        /// Has the order been entirely delivered ?
        /// </summary>
        public bool HasBeenEntirelyDelivered { get; set; }
    }
}
