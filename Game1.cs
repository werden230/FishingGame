// Game1.cs
using System;
using System.Collections.Generic;
using FishingGame.FishSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishingGame
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Inventory _inventory;
        private readonly Random _random;

        private SpriteBatch _spriteBatch;
        private IGameState _currentState;
        private Dictionary<Type, IGameState> _states;
        private Texture2D _whiteTexture;
        private int _money;

        private const int WindowWidth = 720;
        private const int WindowHeight = 1280;
        private static readonly bool SeedTestFishOnStart = true;
        private const int TestFishCount = 25;

        // Публичные свойства
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice;
        public Inventory Inventory => _inventory;
        public Texture2D WhiteTexture => _whiteTexture;
        public int Money => _money;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();

            _inventory = new Inventory();
            _random = new Random();
            _money = 0;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });

            SeedTestFish();

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

        public void AddMoney(int amount)
        {
            _money += amount;
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

        private void SeedTestFish()
        {
            if (!SeedTestFishOnStart)
            {
                return;
            }

            BiomeType[] biomeTypes =
            {
                BiomeType.Ocean,
                BiomeType.River,
                BiomeType.Lake
            };

            for (int i = 0; i < TestFishCount; i++)
            {
                BiomeType biomeType = biomeTypes[_random.Next(biomeTypes.Length)];
                IFishFactory factory = FishFactoryProvider.GetFactory(biomeType);
                Fish fish = factory.CreateRandomFish();
                _inventory.AddFish(fish);
            }
        }
    }
}