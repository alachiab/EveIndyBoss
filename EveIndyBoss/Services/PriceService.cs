using System.Threading.Tasks;
using eZet.EveLib.EveCentralModule.Models;

namespace EveIndyBoss.Services
{
    public interface IHavePrices
    {
        Task<QuicklookResult> GetPrice(int typeId, int solarSystem);
    }

    public class PriceService : IHavePrices
    {
        private readonly ICacheThings _cache;
        private readonly IFetchMarketData _market;

        public PriceService(ICacheThings cache, IFetchMarketData market)
        {
            _cache = cache;
            _market = market;
        }

        public async Task<QuicklookResult> GetPrice(int typeId, int solarSystem)
        {
            var cacheKey = CreateCacheKey(typeId, solarSystem);

            var fromCache = await _cache.Get<QuicklookResult>(cacheKey);
            if (fromCache.Found)
                return fromCache.Data;

            var fromMarket = await _market.GetAsync(typeId, solarSystem);
            await _cache.Set(cacheKey, fromMarket);

            return fromMarket;
        }

        private static string CreateCacheKey(int typeId, int location)
        {
            return $"{typeId}_{location}";
        }
    }
}