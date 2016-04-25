using System.Collections.Generic;
using System.Threading.Tasks;
using eZet.EveLib.EveCentralModule;
using eZet.EveLib.EveCentralModule.Models;

namespace EveIndyBoss.Services
{
    public interface IFetchMarketData
    {
        Task<QuicklookResult> GetAsync(int typeId, int solarSystem);
    }

    public class EveCentralPriceService : IFetchMarketData
    {
        private readonly EveCentral _eveCentral;

        public EveCentralPriceService(EveCentral eveCentral)
        {
            _eveCentral = eveCentral;
        }

        public async Task<QuicklookResult> GetAsync(int typeId, int solarSystem)
        {
            var options = new EveCentralOptions
            {
                Items = new List<int> {typeId},
                System = solarSystem
            };

            var results = await _eveCentral.GetQuicklookAsync(options);

            return results.Result;
        }
    }
}