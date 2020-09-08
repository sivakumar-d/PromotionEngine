//-----------------------------------------------------------------------
// <copyright file="PromotionEnginManager.cs" company="Maersk">
// Copyright (c) Company. All rights reserved.
// </copyright>
// <author>Siva Kumar Reddy</author>
//-----------------------------------------------------------------------

namespace PromotionEngine.BusinessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using PromotionEngine.Entities;
    using PromotionEngine.Interfaces;

    /// <summary>
    /// The Promotion Engine Methods
    /// </summary>
    public class PromotionEnginManager : IPromotionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionEnginManager"/> class.
        /// </summary>
        public PromotionEnginManager()
        {
        }

        /// <summary>
        /// Gets the Total Price by orders and promotions.
        /// </summary>
        /// <param name="ordersData">The orders Data.</param>
        /// <param name="promotionsData">The promotions data.</param>
        /// <returns>
        /// The Final Price
        /// </returns>
        public decimal GetTotalPrice(List<Order> ordersData, List<Promotion> promotionsData)
        {
            decimal resultPrice = 0M;
            foreach (Order ord in ordersData)
            {
                List<decimal> promoprices = promotionsData
                    .Select(promo => GetPrice(ord, promo))
                    .ToList();
                resultPrice += promoprices.Sum();
            }
            
            return resultPrice;
        }

        /// <summary>
        /// Get the Price for each promotion
        /// </summary>
        /// <param name="order">The Order</param>
        /// <param name="promotion">The Promotion</param>
        /// <returns>The Final Price</returns>
        private static decimal GetPrice(Order order, Promotion promotion)
        {
            decimal price = 0M;
            var copp = order.Products
                .GroupBy(x => x.Id)
                .Where(grp => promotion.ProductInfo.Any(y => grp.Key == y.Key && grp.Count() >= y.Value))
                .Select(grp => grp.Count())
                .Sum();
            int ppc = promotion.ProductInfo.Sum(kvp => kvp.Value);
            while (copp >= ppc)
            {
                price += promotion.PromoPrice;
                copp -= ppc;
            }

            return price;
        }
    }
}
