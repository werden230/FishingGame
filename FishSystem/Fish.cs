using Microsoft.Xna.Framework.Graphics;

namespace FishingGame.FishSystem
{
    public class Fish
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public Texture2D Texture { get; set; }
        public FishRarity Rarity { get; set; }
        public int MinimumSize { get; set; }
        public int MaximumSize { get; set; }
        public float Weight { get; set; }
        public int ExperienceReward { get; set; }
        // public Dictionary<string, object> SpecialProperties { get; set; }
        
        public Fish()
        {
            // SpecialProperties = new Dictionary<string, object>();
        }
        
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
    
    public enum FishHabitat
    {
        Ocean,
        River,
        Lake
        // Pond,
        // Sea
    }
}