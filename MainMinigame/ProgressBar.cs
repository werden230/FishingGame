using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.Observer;
using System;

namespace FishingMiniGame
{
    public class ProgressBar : IObserver
    {
        private Texture2D texture;
        private Rectangle bounds;
        private float progress;
        
        private const float ProgressGainRate = 0.002f;
        private const float ProgressLossRate = 0.004f;
        
        public ProgressBar(Texture2D whiteTexture, Rectangle bounds)
        {
            this.texture = whiteTexture;
            this.bounds = bounds;
            this.progress = 0.5f;
        }
        
        public void Increase(float amount)
        {
            progress += amount;
            if (progress > 1f) progress = 1f;
        }
        
        public void Decrease(float amount)
        {
            progress -= amount;
            if (progress < 0f) progress = 0f;
        }
        
        public bool IsComplete()
        {
            return progress >= 1f;
        }
        
        public bool IsEmpty()
        {
            return progress <= 0f;
        }
        
        public void Reset()
        {
            progress = 0f;
        }
        
        public void OnNotify(GameEvent gameEvent, object data = null)
        {
            switch (gameEvent)
            {
                case GameEvent.BarFishContact:
                case GameEvent.BarFishContactUpdate:
                    Increase(ProgressGainRate);
                    break;
                    
                case GameEvent.BarFishLost:
                    Decrease(ProgressLossRate);
                    break;
            }
        }

        public void UpdateContact(bool isInContact)
        {
            if (isInContact)
            {
                Increase(ProgressGainRate);
            }
            else
            {
                Decrease(ProgressLossRate);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            int filledHeight = (int)(bounds.Height * progress);
            int emptyHeight = bounds.Height - filledHeight;
            
            if (filledHeight > 0)
            {
                Rectangle filledRect = new Rectangle(
                    bounds.X,
                    bounds.Y + bounds.Height - filledHeight,
                    bounds.Width,
                    filledHeight
                );
                spriteBatch.Draw(texture, filledRect, Color.Lime);
            }
        }
    }
}