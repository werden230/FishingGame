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
                director.ConstructBasicFish("Tuna", FishRarity.Common, "SIN(2, 30)"),
                director.ConstructBasicFish("Anchovy", FishRarity.Common, "COS(4, 35)"),
                director.ConstructBasicFish("Sardine", FishRarity.Uncommon, "JUMP(1, 70)"),
                director.ConstructBasicFish("Pufferfish", FishRarity.Uncommon, "SIN(3, 40) + JUMP(1, 40)"),
                director.ConstructBasicFish("Red Mullet", FishRarity.Rare, "SIN(4, 100)"),
                director.ConstructBasicFish("Herring", FishRarity.Rare, "COS(2, 50) + RANDOM(100, 1)"),
                director.ConstructBasicFish("Albacore", FishRarity.Epic, "SIN(1.5, 45) + JUMP(0.5, 50)"),
                director.ConstructBasicFish("Eel", FishRarity.Epic, "SIN(1.5, 80) + JUMP(5, 0.4)"),
                director.ConstructBasicFish("Octopus", FishRarity.Legendary, "SIN(5, 80) + JUMP(2, 0.6)")
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
            else 
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
                .SetSize(template.Size + _random.Next(-5, 15))
                .SetWeight(template.Weight + (float)_random.NextDouble() * 2 - 1)
                .SetExperience(template.ExperienceReward)
                .SetMovementPattern(template.MovementPattern)
                .Build();
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
                director.ConstructBasicFish("Salmon", FishRarity.Common, "SIN(2, 30)"),
                director.ConstructBasicFish("Sunfish", FishRarity.Common, "COS(4, 35)"),
                director.ConstructBasicFish("Tiger Trout", FishRarity.Uncommon, "JUMP(1, 70)"),
                director.ConstructBasicFish("Dorado", FishRarity.Uncommon, "SIN(3, 40) + JUMP(1, 40)"),
                director.ConstructBasicFish("Bream", FishRarity.Rare, "SIN(4, 100)"),
                director.ConstructBasicFish("Shad", FishRarity.Rare, "COS(2, 50) + RANDOM(100, 1)"),
                director.ConstructBasicFish("Smallmouth Bass", FishRarity.Epic, "SIN(1.5, 45) + JUMP(0.5, 50)"),
                director.ConstructBasicFish("Pike", FishRarity.Epic, "SIN(1.5, 80) + JUMP(5, 0.4)"),
                director.ConstructBasicFish("Midnight Carp", FishRarity.Legendary, "SIN(5, 80) + JUMP(2, 0.6)")
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

            int size = template.Size + _random.Next(-3, 10);

            return builder
                .SetName(template.Name)
                .SetRarity(template.Rarity)
                .SetPrice(template.Price*(size/10))
                .SetSize(size)
                .SetWeight(template.Weight + (float)_random.NextDouble() * 1.5f - 0.75f)
                .SetExperience(template.ExperienceReward)
                .SetMovementPattern(template.MovementPattern)
                .Build();
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
                director.ConstructBasicFish("Largemouth Bass", FishRarity.Common, "SIN(2, 30)"),
                director.ConstructBasicFish("Sturgeon", FishRarity.Common, "COS(4, 35)"),
                director.ConstructBasicFish("Rainbow Trout", FishRarity.Uncommon, "JUMP(1, 70)"),
                director.ConstructBasicFish("Walleye", FishRarity.Uncommon, "SIN(3, 40) + JUMP(1, 40)"),
                director.ConstructBasicFish("Carp", FishRarity.Rare, "SIN(4, 100)"),
                director.ConstructBasicFish("Perch", FishRarity.Rare, "COS(2, 50) + RANDOM(100, 1)"),
                director.ConstructBasicFish("Bullhead", FishRarity.Epic, "SIN(1.5, 45) + JUMP(0.5, 50)"),
                director.ConstructBasicFish("Chub", FishRarity.Epic, "SIN(1.5, 80) + JUMP(5, 0.4)"),
                director.ConstructBasicFish("Lingcod", FishRarity.Legendary, "SIN(5, 80) + JUMP(2, 0.6)")
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

            int size = template.Size + _random.Next(-4, 12);

            return builder
                .SetName(template.Name)
                .SetRarity(template.Rarity)
                .SetPrice(template.Price*(size/10))
                .SetSize(size)
                .SetWeight(template.Weight + (float)_random.NextDouble() * 2 - 1)
                .SetExperience(template.ExperienceReward)
                .SetMovementPattern(template.MovementPattern)
                .Build();
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