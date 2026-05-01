using Microsoft.Xna.Framework.Graphics;

namespace FishingGame.FishSystem
{
    public class Fish
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public Texture2D Texture { get; set; }
        public FishRarity Rarity { get; set; }
        public int Size { get; set; }
        public float Weight { get; set; }
        public int ExperienceReward { get; set; }
        public string MovementPattern { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Rarity}) - ${Price} - {Weight}kg";
        }
    }
    
    public enum FishRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}