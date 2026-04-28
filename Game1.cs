using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.MiniGames;

namespace FishingGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Sprite fishingBGSprite;
        private FishingMiniGameLogic fishingMiniGame;  // ← Изменено здесь
        private Texture2D whiteTexture;
        
        private const int WindowWidth = 720;
        private const int WindowHeight = 1280;
        private Vector2 fishingMiniGamePosition = new Vector2(100, 50);

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Texture2D fishingBGTexture = Content.Load<Texture2D>("Fishing_BG");
            Texture2D fishingBar = Content.Load<Texture2D>("Fishing_Bar");
            Texture2D fishingFish = Content.Load<Texture2D>("Fishing_Fish");
            
            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Color.White });
            
            fishingBGSprite = new Sprite(fishingBGTexture, fishingMiniGamePosition);
            
            // Создаём мини-игру - используем правильное имя класса
            fishingMiniGame = new FishingMiniGameLogic(  // ← Изменено здесь
                fishingMiniGamePosition,
                fishingBar,
                fishingFish,
                whiteTexture
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (fishingMiniGame != null)
            {
                fishingMiniGame.Update(gameTime);
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(fishingBGSprite.texture, fishingBGSprite.position, Color.White);
            fishingMiniGame?.Draw(_spriteBatch);
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}