using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FishingMiniGame.Entities
{
    public class Fish
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; private set; }
        public float Y => Position.Y;
        
        private float velocity;
        private float minY;
        private float maxY;
        private Random random = new Random();
        
        public Fish(Texture2D texture, Vector2 startPosition, float minY, float maxY)
        {
            Texture = texture;
            Position = startPosition;
            this.minY = minY;
            this.maxY = maxY;
            velocity = 0;
        }
        
        public void Update()
        {
            // Случайное движение
            velocity += (float)(random.NextDouble() - 0.5) * 0.5f;
            velocity = MathHelper.Clamp(velocity, -3f, 3f);
            
            float newY = Position.Y + velocity;
            
            // Ограничение границами с отскоком
            if (newY < minY)
            {
                newY = minY;
                velocity = Math.Abs(velocity) * 0.5f;
            }
            
            if (newY > maxY)
            {
                newY = maxY;
                velocity = -Math.Abs(velocity) * 0.5f;
            }
            
            Position = new Vector2(Position.X, newY);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}