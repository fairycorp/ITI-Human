using static ITI.Human.ViewModels.Product.Ordered.DetailedDataOrderedProduct;

namespace ITI.Human.ViewModels.Order.Payment
{
    public class PaymentStateUpdateViewModel
    {
        public int UserId { get; set; }

        public int OrderedProductId { get; set; }

        public PaymentState PaymentState { get; set; }
    }
}
