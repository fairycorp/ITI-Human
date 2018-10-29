//using API.ViewModels.Order;
//using API.ViewModels.Product.Ordered;
//using System;
//using System.Collections.Generic;

//namespace API.Services.Helper
//{
//    public class DataBuilder
//    {
//        public DataBuilder()
//        {
//            DetailedDataOrder detailedOrder = new DetailedDataOrder
//            {
//                OrderInfo = new BasicDataOrder
//                {
//                    OrderId = 4,
//                    UserId = 10,
//                    CreationDate = DateTime.Now,
//                    HasBeenEntirelyDelivered = false
//                },

//                Products = new List<BasicDataOrderedProduct>
//                {
//                    new BasicDataOrderedProduct
//                    {
//                        OrderedProductId = 3,
//                        OrderId = 4,
//                        ProductId = 12,
//                        Name = "Kinder Bueno",
//                        Desc = "A delicious chocolate bar.",
//                        HasBeenDelivered = false
//                    },

//                    new BasicDataOrderedProduct
//                    {
//                        OrderedProductId = 6,
//                        OrderId = 4,
//                        ProductId = 14,
//                        Name = "Kinder Country",
//                        Desc = "Chocolate from the country.",
//                        HasBeenDelivered = false
//                    },

//                    new BasicDataOrderedProduct
//                    {
//                        OrderedProductId = 3,
//                        OrderId = 4,
//                        ProductId = 9,
//                        Name = "Redbull (33cl)",
//                        Desc = "Energizing drink.",
//                        HasBeenDelivered = true
//                    },
//                }
//            };
//            Fragment(detailedOrder);
//        }

//        private object Fragment(object data, int level = 0)
//        {
//            var final = new Dictionary<string, object>();
//            foreach (var property in data.GetType().GetProperties())
//            {
//                Dictionary<string, object> content = new Dictionary<string, object>();
//                if (!(property.GetValue(data).GetType() is object))
//                {
//                    content.Add(property.Name, property.GetValue(data));
//                }
//                else
//                {
//                    level++;
//                    // content = Fragment(ToObject(property.GetValue(data), level));
//                    //content = Fragment(property.GetValue(data), level);
//                }
//                //register.Add(property.Name, property.GetValue(data));
//            }
//            //return register;
//        }

//        private object ToObject()
//        {

//        }
        
//    }
//}
