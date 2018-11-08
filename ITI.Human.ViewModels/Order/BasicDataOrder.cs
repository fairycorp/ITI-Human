using System;

namespace API.ViewModels.Order
{
    public enum State {
        NotStarted, Underway, Paused, Delivered, Canceled
    }

    public enum Mode {
        Takeaway, Delivery
    }

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
        /// Client name, who is a user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Name of the classroom where to deliver the Order.
        /// </summary>
        /// <remarks>0 if mode is takeaway.</remarks>
        public string ClassroomName { get; set; }

        /// <summary>
        /// Order creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Order current mode.
        /// </summary>
        public Mode CurrentMode { get; set; }

        /// <summary>
        /// Order current state.
        /// </summary>
        public State CurrentState { get; set; }

        /// <summary>
        /// Order total price.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Has the order been entirely delivered ?
        /// </summary>
        public bool HasBeenEntirelyDelivered { get; set; }
    }
}
