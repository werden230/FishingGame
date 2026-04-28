using Microsoft.Xna.Framework.Graphics;

namespace FishingGame.FishSystem
{
    public interface IFishBuilder
    {
        IFishBuilder SetName(string name);
        IFishBuilder SetPrice(int price);
        IFishBuilder SetTexture(Texture2D texture);
        IFishBuilder SetRarity(FishRarity rarity);
        IFishBuilder SetSize(int minSize, int maxSize);
        IFishBuilder SetWeight(float weight);
        IFishBuilder SetExperience(int exp);
        // IFishBuilder AddSpecialProperty(string key, object value);
        Fish Build();
    }
    
    public class FishBuilder : IFishBuilder
    {
        private Fish _fish;
        
        public FishBuilder()
        {
            Reset();
        }
        
        public void Reset()
        {
            _fish = new Fish();
        }
        
        public IFishBuilder SetName(string name)
        {
            _fish.Name = name;
            return this;
        }
        
        public IFishBuilder SetPrice(int price)
        {
            _fish.Price = price;
            return this;
        }
        
        public IFishBuilder SetTexture(Texture2D texture)
        {
            _fish.Texture = texture;
            return this;
        }
        
        public IFishBuilder SetRarity(FishRarity rarity)
        {
            _fish.Rarity = rarity;
            
            // Автоматическая настройка цены на основе редкости
            if (_fish.Price == 0)
            {
                _fish.Price = GetBasePriceByRarity(rarity);
            }
            
            return this;
        }
        
        public IFishBuilder SetSize(int minSize, int maxSize)
        {
            _fish.MinimumSize = minSize;
            _fish.MaximumSize = maxSize;
            return this;
        }
        
        public IFishBuilder SetWeight(float weight)
        {
            _fish.Weight = weight;
            return this;
        }
        
        public IFishBuilder SetExperience(int exp)
        {
            _fish.ExperienceReward = exp;
            return this;
        }
        
        // public IFishBuilder AddSpecialProperty(string key, object value)
        // {
        //     _fish.SpecialProperties[key] = value;
        //     return this;
        // }
        
        public Fish Build()
        {
            Fish result = _fish;
            Reset(); // Сброс для следующего использования
            return result;
        }
        
        private int GetBasePriceByRarity(FishRarity rarity)
        {
            switch (rarity)
            {
                case FishRarity.Common: return 10;
                case FishRarity.Uncommon: return 30;
                case FishRarity.Rare: return 100;
                case FishRarity.Epic: return 300;
                case FishRarity.Legendary: return 1000;
                default: return 10;
            }
        }
    }
    
    // Директор для создания предустановленных рыб
    public class FishDirector
    {
        private IFishBuilder _builder;
        
        public FishDirector(IFishBuilder builder)
        {
            _builder = builder;
        }
        
        public Fish ConstructBasicFish(string name, FishRarity rarity)
        {
            return _builder
                .SetName(name)
                .SetRarity(rarity)
                .SetPrice(GetPriceByRarity(rarity))
                .SetSize(10, 50)
                .SetWeight(1.0f)
                .SetExperience(GetExperienceByRarity(rarity))
                .Build();
        }
        
        public Fish ConstructTrophyFish(string name, FishRarity rarity)
        {
            return _builder
                .SetName(name)
                .SetRarity(rarity)
                .SetPrice(GetPriceByRarity(rarity) * 3)
                .SetSize(50, 200)
                .SetWeight(10.0f)
                .SetExperience(GetExperienceByRarity(rarity) * 2)
                // .AddSpecialProperty("IsTrophy", true)
                // .AddSpecialProperty("Ribbon", "Gold")
                .Build();
        }
        
        private int GetPriceByRarity(FishRarity rarity)
        {
            switch (rarity)
            {
                case FishRarity.Common: return 10;
                case FishRarity.Uncommon: return 30;
                case FishRarity.Rare: return 100;
                case FishRarity.Epic: return 300;
                case FishRarity.Legendary: return 1000;
                default: return 10;
            }
        }
        
        private int GetExperienceByRarity(FishRarity rarity)
        {
            switch (rarity)
            {
                case FishRarity.Common: return 5;
                case FishRarity.Uncommon: return 15;
                case FishRarity.Rare: return 50;
                case FishRarity.Epic: return 150;
                case FishRarity.Legendary: return 500;
                default: return 5;
            }
        }
    }
}