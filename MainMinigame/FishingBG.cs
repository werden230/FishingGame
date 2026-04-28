using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FishingMiniGame.Entities
{
    public class FishingBG
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; private set; }
        
        public FishingBG(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}