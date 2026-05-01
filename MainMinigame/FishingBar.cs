using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.Observer;
using System.Collections.Generic;

namespace FishingMiniGame.Entities
{
    public class FishingBar : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();
        
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Height => Texture?.Height ?? 108;
        public float Width => Texture?.Width ?? 27;
        
        private float minY;
        private float maxY;
        private float previousY;
        
        public FishingBar(Texture2D texture, Vector2 startPosition, float minY, float maxY)
        {
            Texture = texture;
            Position = startPosition;
            previousY = startPosition.Y;
            this.minY = minY;
            this.maxY = maxY;
        }
        
        public void UpdatePosition(float newY)
        {
            previousY = Position.Y;
            Position = new Vector2(Position.X, MathHelper.Clamp(newY, minY, maxY));
            
            if (previousY != Position.Y)
            {
                Notify(GameEvent.BarPositionChanged, Position.Y);
            }
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
        
        public void Attach(IObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }
        
        public void Detach(IObserver observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }
        
        public void Notify(GameEvent gameEvent, object data = null)
        {
            foreach (var observer in observers)
            {
                observer.OnNotify(gameEvent, data);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}