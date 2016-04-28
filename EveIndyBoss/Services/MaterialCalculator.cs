using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveIndyBoss.Infrastructure;
using EveIndyBoss.Models;
using EveIndyBoss.Models.StaticData;
using EveIndyBoss.Services.StaticData;

namespace EveIndyBoss.Services
{
    public interface ICalculateMaterials
    {
        IEnumerable<MaterialForProduction> Calculate(List<MaterialForProduction> mats, int efficiency,
            int amount);

        Task<IEnumerable<MaterialForProduction>> GetCalculationBase(Blueprint blueprint);
        ProductInfo CalculateStuff(List<MaterialForProduction> mats, int productTypeId, int quantity);
    }

    public class MaterialCalculator : ICalculateMaterials
    {
        private const int PerM3 = 300;
        private const decimal CollPerIsk = 0.02m;
        private readonly IHavePrices _market;
        private readonly IProvideStaticData _staticData;
        private readonly ICacheThings _cache;

        public MaterialCalculator(IProvideStaticData staticData, IHavePrices market, ICacheThings cache)
        {
            _staticData = staticData;
            _market = market;
            _cache = cache;
        }

        public async Task<IEnumerable<MaterialForProduction>> GetCalculationBase(Blueprint blueprint)
        {
            if (blueprint == null)
                return new List<MaterialForProduction>();

            var materials = await _staticData.GetMaterialsForTypeAsync(blueprint.TypeId, 1);
            var data = new List<MaterialForProduction>();

            foreach (var materialsForType in materials)
            {
                var priceInfo = await _market.GetPrice(materialsForType.MaterialTypeId, Constants.Systems.Jita);
                decimal price = 0;
                if (priceInfo.SellOrders.Any())
                    price = priceInfo.SellOrders.First().Price;

                var matInfo = new MaterialForProduction
                {
                    TypeId = materialsForType.MaterialTypeId,
                    Cost = price,
                    Name = materialsForType.MaterialName,
                    Volume = materialsForType.Volume,
                    Quantity = materialsForType.Quantity
                };

                data.Add(matInfo);
            }

            return data;
        }

        public ProductInfo CalculateStuff(List<MaterialForProduction> mats, int productTypeId, int quantity)
        {
            var jita = _market.GetPrice(productTypeId, Constants.Systems.Jita).Result;
            var staging = _market.GetPrice(productTypeId, Constants.Systems.O1Y).Result;

            var totalCost = mats.Sum(x => x.CostTotal);
            var jitaSellPrice = jita.GetFirstSellPrice();

            var item = _cache.Get($"item_{productTypeId}", () => _staticData.GetSingeItemAsync(productTypeId)).Result;
            
            var volume = Constants.PackagedVolumes.ContainsKey(item.Data.GroupId)
                ? Constants.PackagedVolumes[item.Data.GroupId]
                : item.Data.Volume;

            return new ProductInfo
            {
                JitaSell = jitaSellPrice,
                ShippingToStaging = volume * PerM3,
                StagingSell = staging.GetFirstSellPrice(),
                SellAtProfit = (totalCost * 1.1m)/ quantity,
                TotalCost = totalCost,
                TotalCostPerItem = totalCost / quantity
            };
        }

        public IEnumerable<MaterialForProduction> Calculate(List<MaterialForProduction> mats,
            int efficiency, int amount)
        {
            var data = new List<MaterialForProduction>();

            if (!mats.Any())
                return data;

            foreach (var materialsForType in mats)
            {
                var actualEfficiency = (1.0 - (efficiency/100.0));
                var quantityPrecise = (materialsForType.Quantity*amount)*actualEfficiency;
                var quantity = Convert.ToInt32(Math.Ceiling(quantityPrecise));

                var costTotal = quantity*materialsForType.Cost;
                var totalVolume = quantity*materialsForType.Volume;

                var shipCostForVolume = totalVolume*PerM3;
                var collateralCost = (costTotal + shipCostForVolume)*CollPerIsk;

                var matInfo = new MaterialForProduction
                {
                    TypeId = materialsForType.TypeId,
                    Name = materialsForType.Name,
                    Quantity = quantity,
                    Cost = materialsForType.Cost,
                    Volume = materialsForType.Volume,
                    CostTotal = costTotal,
                    VolumeAll = totalVolume,
                    Shipping = shipCostForVolume,
                    Collateral = collateralCost,
                    CostAll = costTotal + shipCostForVolume + collateralCost
                };

                data.Add(matInfo);
            }

            return data;
        }
    }
}