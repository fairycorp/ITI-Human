using System;

namespace Fork.ViewModels.Order
{
    /// <summary>
    /// Defines what an Order current State is.
    public enum State {
        NotStarted, Underway, Paused, Delivered, Canceled
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
        /// Matching storage id.
        /// </summary>
        public int StorageId { get; set; }

        /// <summary>
        /// Client id, who is a user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Client name, who is a user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Id of the classroom where to deliver the Order.
        /// </summary>
        public int ClassroomId { get; set; }

        /// <summary>
        /// Name of the classroom where to deliver the Order.
        /// </summary>
        /// <remarks>Null if ClassroomId is 0.</remarks>
        public string ClassroomName { get; set; }

        /// <summary>
        /// Order creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Order current state.
        /// </summary>
        public State CurrentState { get; set; }

        /// <summary>
        /// Order total price.
        /// </summary>
        public int Total { get; set; }
    }
}
