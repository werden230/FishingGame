using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishingMiniGame.Entities
{
    public class ProgressBar
    {
        private Rectangle background;
        private Rectangle fill;
        private Texture2D whiteTexture;
        private float progress = 0.5f;
        
        public float Progress 
        { 
            get => progress;
            set => progress = MathHelper.Clamp(value, 0, 1);
        }
        
        public ProgressBar(Texture2D whiteTexture, Rectangle backgroundRect)
        {
            this.whiteTexture = whiteTexture;
            this.background = backgroundRect;
            UpdateFill();
        }
        
        public void Increase(float amount)
        {
            Progress += amount;
            UpdateFill();
        }
        
        public void Decrease(float amount)
        {
            Progress -= amount;
            UpdateFill();
        }
        
        private void UpdateFill()
        {
            int fillHeight = (int)(background.Height * progress);
            fill = new Rectangle(
                background.X,
                background.Y + background.Height - fillHeight,
                background.Width,
                fillHeight
            );
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(whiteTexture, fill, Color.Lime);
        }
        
        public bool IsComplete() => progress >= 1f;
        public bool IsEmpty() => progress <= 0f;
    }
}