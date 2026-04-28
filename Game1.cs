using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.MiniGames;
using System;
using System.Collections.Generic;

namespace FishingGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private IGameState _currentState;
        private Dictionary<Type, IGameState> _states;
        
        private const int WindowWidth = 720;
        private const int WindowHeight = 1280;
        
        public GraphicsDevice GraphicsDeviceInstance => GraphicsDevice;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Инициализируем состояния
            _states = new Dictionary<Type, IGameState>
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