namespace API.ViewModels.Product.Ordered
{
    public class CreationViewModel
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public bool HasBeenDelivered { get; set; }
    }
}
