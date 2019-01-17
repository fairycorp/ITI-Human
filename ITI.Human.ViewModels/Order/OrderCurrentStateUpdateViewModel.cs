namespace ITI.Human.ViewModels.Order
{
    public class OrderCurrentStateUpdateViewModel
    {
        public int UserId { get; set; }

        public int OrderId { get; set; }

        public State CurrentState { get; set; }
    }
}
