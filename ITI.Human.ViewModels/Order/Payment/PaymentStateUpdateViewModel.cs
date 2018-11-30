using static ITI.Human.ViewModels.Product.Ordered.BasicDataOrderedProduct;

namespace ITI.Human.ViewModels.Order.Payment
{
    public class PaymentStateUpdateViewModel
    {
        public int OrdereredProductId { get; set; }

        public PaymentState PaymentState { get; set; }
    }
}
