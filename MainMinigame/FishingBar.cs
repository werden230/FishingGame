using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishingMiniGame.Entities
{
    public class FishingBar
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Height => Texture?.Height ?? 108; //108
        public float Width => Texture?.Width ?? 27; //27
        
        private float minY;
        private float maxY;
        
        public FishingBar(Texture2D texture, Vector2 startPosition, float minY, float maxY)
        {
            Texture = texture;
            Position = startPosition;
            this.minY = minY;
            this.maxY = maxY;
        }
        
        public void UpdatePosition(float newY)
        {
            Position = new Vector2(Position.X, MathHelper.Clamp(newY, minY, maxY));
        }
        
        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Width,
                (int)Height
            );
        }
        
        public bool Contains(float yPosition)
        {
            return yPosition >= Position.Y && yPosition <= Position.Y + Height;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}