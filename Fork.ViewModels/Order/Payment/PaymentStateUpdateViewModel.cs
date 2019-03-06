using static Fork.ViewModels.Product.Ordered.DetailedDataOrderedProduct;

namespace Fork.ViewModels.Order.Payment
{
    public class PaymentStateUpdateViewModel
    {
        public int ActorId { get; set; }

        public int UserId { get; set; }

        public int OrderedProductId { get; set; }

        public PaymentState PaymentState { get; set; }
    }
}
