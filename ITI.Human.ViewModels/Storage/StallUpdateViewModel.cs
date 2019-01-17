namespace ITI.Human.ViewModels.Storage
{
    public class StallUpdateViewModel
    {
        /// <summary>
        /// Current User's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Current Storage id.
        /// </summary>
        public int StorageId { get; set; }

        /// <summary>
        /// Does Storage Stall is open ?
        /// </summary>
        public bool OpenState { get; set; }
    }
}
