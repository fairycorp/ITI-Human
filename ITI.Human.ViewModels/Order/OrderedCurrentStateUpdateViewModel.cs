namespace ITI.Human.ViewModels.Order
{
    public class OrderedCurrentStateUpdateViewModel
    {       
        public int UserId { get; set; }

        public int OrderedProductId { get; set; }

        public State CurrentState { get; set; }
    }
}
