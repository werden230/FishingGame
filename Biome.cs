using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FishingGame.FishSystem;
using System.Collections.Generic;

namespace FishingGame
{
    public class Biome
    {
        public string Name { get; set; }
        public BiomeType BiomeType { get; set; }
        public Texture2D BackgroundTexture { get; set; }
        public IFishFactory FishFactory { get; set; }
        
        public Biome(string name, BiomeType biomeType, Texture2D backgroundTexture)
        {
            Name = name;
            BiomeType = biomeType;
            BackgroundTexture = backgroundTexture;
            FishFactory = FishFactoryProvider.GetFactory(biomeType);
        }
        
        public Fish GetRandomFish()
        {
            return FishFactory.CreateRandomFish();
        }
        
        public Fish GetFishByRarity(FishRarity rarity)
        {
            return FishFactory.CreateFishByRarity(rarity);
        }
        
        public List<Fish> GetAllAvailableFish()
        {
            return FishFactory.GetAvailableFish();
        }
    }
}