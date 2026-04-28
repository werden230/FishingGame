// Game1.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishingGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private IGameState _currentState;
        private Dictionary<System.Type, IGameState> _states;
        private Inventory _inventory;
        private Texture2D _whiteTexture;
        
        private const int WindowWidth = 720;
        private const int WindowHeight = 1280;
        
        // Публичные свойства
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice;
        public Inventory Inventory => _inventory;
        public Texture2D WhiteTexture => _whiteTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();
            
            _inventory = new Inventory();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
                        
            _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });
            
            // Инициализируем состояния
            _states = new Dictionary<System.Type, IGameState>
            {
                { typeof(MainGameState), new MainGameState(this) }
            };
            
            // Устанавливаем начальное состояние
            ChangeState<MainGameState>();
        }
        
        public void ChangeState<T>() where T : IGameState
        {
            if (_states.TryGetValue(typeof(T), out var newState))
            {
                ChangeState(newState);
            }
        }
        
        public void ChangeState(IGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            _currentState?.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}