using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Storage.LinkedProduct;

namespace ITI.Human.ViewModels.Product.Ordered
{
    /// <summary>
    /// Defines what an Ordered Product payment state is.
    /// </summary>
    public enum Payment {
        Unpaid, Paid, Credited
    }

    /// <summary>
    /// Represents what an Ordered Product is.
    /// </summary>
    public class DetailedDataOrderedProduct
    {
        /// <summary>
        /// Ordered Product id.
        /// </summary>
        public int OrderedProductId { get; set; }

        /// <summary>
        /// Matching order id.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Matching storage linked product id.
        /// </summary>
        public int StorageLinkedProductId { get; set; }
        
        /// <summary>
        /// See <see cref="BasicDataProduct.Name"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProduct.Desc"/>
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// See <see cref="BasicDataStorageSLP.UnitPrice"/>.
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProductToOrder.Quantity"/>.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Ordered Product current state.
        /// </summary>
        public State CurrentState { get; set; }

        /// <summary>
        /// Ordered Product Payment info.
        /// </summary>
        public PaymentState Payment { get; set; }

        public class PaymentState
        {
            public Payment State { get; set; }
            public int Amount { get; set; }
        }
    }
}
