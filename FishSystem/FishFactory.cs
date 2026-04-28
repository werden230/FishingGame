using System.Collections.Generic;
using System;
using System.Linq;

namespace FishingGame.FishSystem
{
    // Абстрактная фабрика
    public interface IFishFactory
    {
        Fish CreateRandomFish();
        Fish CreateFishByRarity(FishRarity rarity);
        Fish CreateTrophyFish();
        List<Fish> GetAvailableFish();
    }
    
    // Конкретная фабрика для океана
    public class OceanFishFactory : IFishFactory
    {
        private Random _random = new Random();
        private List<Fish> _fishTemplates;
        
        public OceanFishFactory()
        {
            InitializeFishTemplates();
        }
        
        private void InitializeFishTemplates()
        {
            var builder = new FishBuilder();
            var director = new FishDirector(builder);
            
            _fishTemplates = new List<Fish>
            {
                director.ConstructBasicFish("Clownfish", FishRarity.Common),
                director.ConstructBasicFish("Blue Tang", FishRarity.Common),
                director.ConstructBasicFish("Salmon", FishRarity.Uncommon),
                director.ConstructBasicFish("Tuna", FishRarity.Uncommon),
                director.ConstructBasicFish("Swordfish", FishRarity.Rare),
                director.ConstructBasicFish("Marlin", FishRarity.Rare),
                director.ConstructBasicFish("Shark", FishRarity.Epic),
                director.ConstructBasicFish("Octopus", FishRarity.Epic),
                director.ConstructBasicFish("Kraken", FishRarity.Legendary),
                director.ConstructBasicFish("Leviathan", FishRarity.Legendary)
            };
        }
        
        public Fish CreateRandomFish()
        {
            // Шанс выпадения зависит от редкости
            float roll = (float)_random.NextDouble() * 100;
            
            if (roll < 1) // 1% на легендарную
                return CreateFishByRarity(FishRarity.Legendary);
            else if (roll < 5) // 4% на эпическую
                return CreateFishByRarity(FishRarity.Epic);
            else if (roll < 20) // 15% на редкую
                return CreateFishByRarity(FishRarity.Rare);
            else if (roll < 50) // 30% на необычную
                return CreateFishByRarity(FishRarity.Uncommon);
            else // 50% на обычную
                return CreateFishByRarity(FishRarity.Common);
        }
        
        public Fish CreateFishByRarity(FishRarity rarity)
        {
            var fishOfRarity = _fishTemplates.Where(f => f.Rarity == rarity).ToList();
            if (fishOfRarity.Count == 0)
                return _fishTemplates[0];
                
            var template = fishOfRarity[_random.Next(fishOfRarity.Count)];
            
            // Создаем новую рыбу с вариациями размера и веса
            var builder = new FishBuilder();
            return builder
                .SetName(template.Name)
                .SetRarity(template.Rarity)
                .SetPrice(template.Price + _random.Next(-10, 20))
                .SetSize(template.MinimumSize + _random.Next(-5, 15), 
                        template.MaximumSize + _random.Next(-10, 30))
                .SetWeight(template.Weight + (float)_random.NextDouble() * 2 - 1)
                .SetExperience(template.ExperienceReward + _random.Next(-2, 5))
                .Build();
        }
        
        public Fish CreateTrophyFish()
        {
            var builder = new FishBuilder();
            var director = new FishDirector(builder);
            
            var rareFish = _fishTemplates.Where(f => f.Rarity >= FishRarity.Rare).ToList();
            var selectedFish = rareFish[_random.Next(rareFish.Count)];
            
            return director.ConstructTrophyFish($"Golden {selectedFish.Name}", selectedFish.Rarity);
        }
        
        public List<Fish> GetAvailableFish()
        {
            return new List<Fish>(_fishTemplates);
        }
    }
    
    // Фабрика для реки
    public class RiverFishFactory : IFishFactory
    {
        private Random _random = new Random();
        private List<Fish> _fishTemplates;
        
        public RiverFishFactory()
        {
            InitializeFishTemplates();
        }
        
        private void InitializeFishTemplates()
        {
            var builder = new FishBuilder();
            var director = new FishDirector(builder);
            
            _fishTemplates = new List<Fish>
            {
                director.ConstructBasicFish("Perch", FishRarity.Common),
                director.ConstructBasicFish("Roach", FishRarity.Common),
                director.ConstructBasicFish("Trout", FishRarity.Uncommon),
                director.ConstructBasicFish("Grayling", FishRarity.Uncommon),
                director.ConstructBasicFish("Pike", FishRarity.Rare),
                director.ConstructBasicFish("Zander", FishRarity.Rare),
                director.ConstructBasicFish("Catfish", FishRarity.Epic),
                director.ConstructBasicFish("Sturgeon", FishRarity.Epic),
                director.ConstructBasicFish("River Monster", FishRarity.Legendary),
                director.ConstructBasicFish("Golden Trout", FishRarity.Legendary)
            };
        }
        
        public Fish CreateRandomFish()
        {
            float roll = (float)_random.NextDouble() * 100;
            
            if (roll < 1)
                return CreateFishByRarity(FishRarity.Legendary);
            else if (roll < 5)
                return CreateFishByRarity(FishRarity.Epic);
            else if (roll < 20)
                return CreateFishByRarity(FishRarity.Rare);
            else if (roll < 50)
                return CreateFishByRarity(FishRarity.Uncommon);
            else
                return CreateFishByRarity(FishRarity.Common);
        }
        
        public Fish CreateFishByRarity(FishRarity rarity)
        {
            var fishOfRarity = _fishTemplates.Where(f => f.Rarity == rarity).ToList();
            if (fishOfRarity.Count == 0)
                return _fishTemplates[0];
                
            var template = fishOfRarity[_random.Next(fishOfRarity.Count)];
            
            var builder = new FishBuilder();
            return builder
                .SetName(template.Name)
                .SetRarity(template.Rarity)
                .SetPrice(template.Price + _random.Next(-5, 15))
                .SetSize(template.MinimumSize + _random.Next(-3, 10), 
                        template.MaximumSize + _random.Next(-5, 20))
                .SetWeight(template.Weight + (float)_random.NextDouble() * 1.5f - 0.75f)
                .SetExperience(template.ExperienceReward + _random.Next(-2, 4))
                .Build();
        }
        
        public Fish CreateTrophyFish()
        {
            var builder = new FishBuilder();
            var director = new FishDirector(builder);
            
            var rareFish = _fishTemplates.Where(f => f.Rarity >= FishRarity.Rare).ToList();
            var selectedFish = rareFish[_random.Next(rareFish.Count)];
            
            return director.ConstructTrophyFish($"Ancient {selectedFish.Name}", selectedFish.Rarity);
        }
        
        public List<Fish> GetAvailableFish()
        {
            return new List<Fish>(_fishTemplates);
        }
    }
    
    // Фабрика для озера
    public class LakeFishFactory : IFishFactory
    {
        private Random _random = new Random();
        private List<Fish> _fishTemplates;
        
        public LakeFishFactory()
        {
            InitializeFishTemplates();
        }
        
        private void InitializeFishTemplates()
        {
            var builder = new FishBuilder();
            var director = new FishDirector(builder);
            
            _fishTemplates = new List<Fish>
            {
                director.ConstructBasicFish("Carp", FishRarity.Common),
                director.ConstructBasicFish("Crucian", FishRarity.Common),
                director.ConstructBasicFish("Bass", FishRarity.Uncommon),
                director.ConstructBasicFish("Sunfish", FishRarity.Uncommon),
                director.ConstructBasicFish("Walleye", FishRarity.Rare),
                director.ConstructBasicFish("Musky", FishRarity.Rare),
                director.ConstructBasicFish("Lake Trout", FishRarity.Epic),
                director.ConstructBasicFish("Whitefish", FishRarity.Epic),
                director.ConstructBasicFish("Nessie", FishRarity.Legendary),
                director.ConstructBasicFish("Lake Monster", FishRarity.Legendary)
            };
        }
        
        public Fish CreateRandomFish()
        {
            float roll = (float)_random.NextDouble() * 100;
            
            if (roll < 1)
                return CreateFishByRarity(FishRarity.Legendary);
            else if (roll < 5)
                return CreateFishByRarity(FishRarity.Epic);
            else if (roll < 20)
                return CreateFishByRarity(FishRarity.Rare);
            else if (roll < 50)
                return CreateFishByRarity(FishRarity.Uncommon);
            else
                return CreateFishByRarity(FishRarity.Common);
        }
        
        public Fish CreateFishByRarity(FishRarity rarity)
        {
            var fishOfRarity = _fishTemplates.Where(f => f.Rarity == rarity).ToList();
            if (fishOfRarity.Count == 0)
                return _fishTemplates[0];
                
            var template = fishOfRarity[_random.Next(fishOfRarity.Count)];
            
            var builder = new FishBuilder();
            return builder
                .SetName(template.Name)
                .SetRarity(template.Rarity)
                .SetPrice(template.Price + _random.Next(-8, 12))
                .SetSize(template.MinimumSize + _random.Next(-4, 12), 
                        template.MaximumSize + _random.Next(-8, 25))
                .SetWeight(template.Weight + (float)_random.NextDouble() * 2 - 1)
                .SetExperience(template.ExperienceReward + _random.Next(-1, 5))
                .Build();
        }
        
        public Fish CreateTrophyFish()
        {
            var builder = new FishBuilder();
            var director = new FishDirector(builder);
            
            var rareFish = _fishTemplates.Where(f => f.Rarity >= FishRarity.Rare).ToList();
            var selectedFish = rareFish[_random.Next(rareFish.Count)];
            
            return director.ConstructTrophyFish($"Legendary {selectedFish.Name}", selectedFish.Rarity);
        }
        
        public List<Fish> GetAvailableFish()
        {
            return new List<Fish>(_fishTemplates);
        }
    }
    
    // Фабрика провайдер для получения нужной фабрики
    public static class FishFactoryProvider
    {
        public static IFishFactory GetFactory(BiomeType biomeType)
        {
            switch (biomeType)
            {
                case BiomeType.Ocean:
                    return new OceanFishFactory();
                case BiomeType.River:
                    return new RiverFishFactory();
                case BiomeType.Lake:
                    return new LakeFishFactory();
                default:
                    return new OceanFishFactory();
            }
        }
    }
    
    public enum BiomeType
    {
        Ocean,
        River,
        Lake
    }
}