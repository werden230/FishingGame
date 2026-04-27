using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishingMiniGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Sprite fishingBGSprite;
        private Sprite fishingBarSprite;  // green zone
        private Sprite fishingFishSprite;
        private Texture2D _whiteTexture;

        // Размеры окна
        private const int WindowWidth = 720;
        private const int WindowHeight = 1280;

        private Vector2 fishingMiniGamePosition = new Vector2(100, 50);
        
        // Правая полоса прогресса
        private Rectangle progressBarBg;
        private Rectangle progressBarFill;
        private float progress = 0.5f; // Для демонстрации
        
        // Позиция зеленой полоски (fishingBar)
        private Vector2 barPosition;
        
        // Границы движения зеленой полоски
        private float minBarY;
        private float maxBarY;
        
        // Размеры области для рыбки (примерные, подберите под вашу текстуру)
        private Vector2 fishPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // Начальная позиция зеленой полоски
            barPosition = new Vector2(fishingMiniGamePosition.X + 81, fishingMiniGamePosition.Y + 150);
            
            // Правая полоса прогресса
            const int progressBarWidth = 12;
            const int progressBarHeight = 437;
            int progressBarX = (int)fishingMiniGamePosition.X + 126;
            int progressBarY = (int)fishingMiniGamePosition.Y + 15;
    
            progressBarBg = new Rectangle(
                progressBarX,
                progressBarY,
                progressBarWidth,
                progressBarHeight
            );
            
            UpdateProgressBar();
            
            minBarY = fishingMiniGamePosition.Y + 150;
            maxBarY = fishingMiniGamePosition.Y + 150 + 437 - GetBarHeight();
            
            fishPosition = new Vector2(
                fishingMiniGamePosition.X + 81,
                barPosition.Y + GetBarHeight() / 2
            );
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Texture2D fishingBGTexture = Content.Load<Texture2D>("Fishing_BG");
            Texture2D fishingBar = Content.Load<Texture2D>("Fishing_Bar");
            Texture2D fishingFish = Content.Load<Texture2D>("Fishing_Fish");
            
            _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });

            fishingBGSprite = new Sprite(fishingBGTexture, fishingMiniGamePosition);
            fishingBarSprite = new Sprite(fishingBar, barPosition);
            fishingFishSprite = new Sprite(fishingFish, fishPosition);
        }

        private float GetBarHeight()
        {
            return fishingBarSprite?.texture?.Height ?? 80;
        }

        private void UpdateProgressBar()
        {
            int fillHeight = (int)(progressBarBg.Height * progress);
            progressBarFill = new Rectangle(
                progressBarBg.X,
                progressBarBg.Y + progressBarBg.Height - fillHeight,
                progressBarBg.Width,
                fillHeight
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // Временная анимация прогресса для демонстрации
            progress += 0.005f;
            if (progress > 1) progress = 0;
            UpdateProgressBar();
            
            // Обновляем позицию спрайтов
            fishingBarSprite.position = barPosition;
            fishingFishSprite.position = fishPosition;
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(fishingBGSprite.texture, fishingBGSprite.position, Color.White);
            _spriteBatch.Draw(fishingBarSprite.texture, fishingBarSprite.position, Color.White);
            _spriteBatch.Draw(fishingFishSprite.texture, fishingFishSprite.position, Color.White);
            _spriteBatch.Draw(_whiteTexture, progressBarFill, Color.Lime);
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}