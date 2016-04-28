using System.Linq;
using eZet.EveLib.EveCentralModule.Models;

namespace EveIndyBoss.Infrastructure
{
    public static class QuickLookResultExtensions
    {
        public static decimal GetFirstSellPrice(this QuicklookResult result)
        {
            if (!result.SellOrders.Any())
                return 0;

            return result.SellOrders
                .OrderBy(x => x.Price)
                .First()
                .Price;
        }
    }
}