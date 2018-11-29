using System;
using System.Collections.Generic;
using System.Text;
using static ITI.Human.ViewModels.Product.Ordered.BasicDataOrderedProduct;

namespace ITI.Human.ViewModels.Order
{
    public class CurrentStateUpdateViewModel
    {       
        public int OrdererProductId { get; set; }

        public State CurrentState { get; set; }
    }
}
