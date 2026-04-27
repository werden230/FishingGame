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
        private Sprite fishingBarSprite;
        private Sprite fishingFishSprite;

        // Размеры окна
        private const int WindowWidth = 720;
        private const int WindowHeight = 1280;

        private Vector2 fishingMiniGamePosition = new Vector2(100, 50);
        
        // Правая полоса прогресса
        private Rectangle progressBarBg;
        private Rectangle progressBarFill; // Заполненная часть
        private float progress = 0.5f; // Временное значение для демонстрации (50%)
        
        // Границы для зеленой зоны (не выходить за пределы воды)
        private int minGreenY;
        private int maxGreenY;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // Устанавливаем размер окна
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {   
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
            
            progressBarFill = new Rectangle(
                progressBarX,
                progressBarY + (int)(progressBarHeight * (1 - progress)), // Заполнение сверху вниз
                progressBarWidth,
                (int)(progressBarHeight * progress)
            );
            
            // Вычисляем границы для зеленой зоны
            // minGreenY = waterRect.Y;
            // maxGreenY = waterRect.Y + waterRect.Height - GreenZoneHeight;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D fishingBGTexture = Content.Load<Texture2D>("Fishing_BG");
            Texture2D fishingBar = Content.Load<Texture2D>("Fishing_Bar");
            Texture2D fishingFish = Content.Load<Texture2D>("Fishing_Fish");

            fishingBGSprite = new Sprite(fishingBGTexture, fishingMiniGamePosition);
            fishingBarSprite = new Sprite(fishingBar, new Vector2(fishingMiniGamePosition.X + 81, fishingMiniGamePosition.Y + 150));
            fishingFishSprite = new Sprite(fishingFish, new Vector2(fishingMiniGamePosition.X + 81, fishingMiniGamePosition.Y + 75));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // Временное обновление прогресса для демонстрации (можно удалить позже)
            // Просто для анимации, чтобы показать, что полоса работает
            progress += 0.005f;
            if (progress > 1) progress = 0;
            UpdateProgressBar();
            
            base.Update(gameTime);
        }

        private void UpdateProgressBar()
        {
            // Обновляем заполнение полосы прогресса
            int fillHeight = (int)(progressBarBg.Height * progress);
            progressBarFill = new Rectangle(
                progressBarBg.X,
                progressBarBg.Y + progressBarBg.Height - fillHeight, // Заполнение снизу вверх
                progressBarBg.Width,
                fillHeight
            );
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            Texture2D whiteTexture = CreateWhiteTexture();

            _spriteBatch.Draw(fishingBGSprite.texture, fishingBGSprite.position, Color.White);

            _spriteBatch.Draw(fishingBarSprite.texture, fishingBarSprite.position, Color.White);

            _spriteBatch.Draw(fishingFishSprite.texture, fishingFishSprite.position, Color.White);
            
            // 4. Рисуем правую полосу прогресса
            _spriteBatch.Draw(whiteTexture, progressBarFill, Color.Lime);

            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
        // Вспомогательный метод для создания белой текстуры 1x1
        // private Texture2D _whiteTexture;
        private Texture2D CreateWhiteTexture()
        {
            Texture2D _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });
            return _whiteTexture;
        }
    }
}