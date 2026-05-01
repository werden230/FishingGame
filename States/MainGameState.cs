using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FishingGame.FishSystem;

namespace FishingGame
{
    public class MainGameState : GameState
    {
        private Game1 _game;
        private List<Biome> _biomes;
        private int _currentBiomeIndex;
        private SpriteFont _font;
        
        private bool IsMousePressed = false;

        private Texture2D _nextButtonTexture;
        private Texture2D _prevButtonTexture;

        private Rectangle _nextButton;
        private Rectangle _prevButton;
        
        public MainGameState(Game1 game)
        {
            _game = game;
            _currentBiomeIndex = 0;

            _nextButtonTexture = _game.Content.Load<Texture2D>("NextButton");
            _prevButtonTexture = _game.Content.Load<Texture2D>("PrevButton");

            _nextButton = new Rectangle(720-50-81, 600, 81, 75);
            _prevButton = new Rectangle(50, 600, 81, 75);
        }
        
        public override void Enter()
        {
            _font = _game.Content.Load<SpriteFont>("DefaultFont");
            InitializeBiomes();
        }
        
        private void InitializeBiomes()
        {
            _biomes = new List<Biome>();
            
            var oceanBg = _game.Content.Load<Texture2D>("ocean_day_bg");
            var riverBg = _game.Content.Load<Texture2D>("river_day_bg");
            var lakeBg = _game.Content.Load<Texture2D>("lake_day_bg");
            
            _biomes.Add(new Biome("Ocean", BiomeType.Ocean, oceanBg));
            _biomes.Add(new Biome("River", BiomeType.River, riverBg));
            _biomes.Add(new Biome("Lake", BiomeType.Lake, lakeBg));
        }
        
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            

            if (!IsMousePressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                IsMousePressed = true;
                Point mousePos = new Point(mouseState.X, mouseState.Y);
                Console.WriteLine("Mouse is pressed");
                
                if (_nextButton.Contains(mousePos))
                    _currentBiomeIndex = (_currentBiomeIndex+1)%3;
                else if (_prevButton.Contains(mousePos))
                    _currentBiomeIndex = _currentBiomeIndex==0 ? 2 : _currentBiomeIndex-1;

            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                IsMousePressed = false;
            }
            
            if (keyState.IsKeyDown(Keys.Space))
            {
                Biome currentBiome = _biomes[_currentBiomeIndex];
                _game.ChangeState(new FishingMiniGameState(_game, currentBiome));
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Biome currentBiome = _biomes[_currentBiomeIndex];
            spriteBatch.Draw(currentBiome.BackgroundTexture, new Rectangle(0, 0, 720,  1280), Color.White);

            spriteBatch.Draw(_nextButtonTexture, _nextButton, Color.White);
            spriteBatch.Draw(_prevButtonTexture, _prevButton, Color.White);

            // DrawButton(spriteBatch, _nextButton, "Next");
            // DrawButton(spriteBatch, _prevButton, "Previous");
            
            spriteBatch.DrawString(_font, "Press SPACE to fish!", 
                new Vector2(250, 700), Color.Black);
            
            DrawInventory(spriteBatch);
            DrawFishCollection(spriteBatch);
        }
        
        // private void DrawButton(SpriteBatch spriteBatch, Rectangle rect, string text)
        // {
        //     Color color = Color.Gold;
        //     spriteBatch.Draw(_game.WhiteTexture, rect, color);
        //     spriteBatch.DrawString(_font, text, 
        //         new Vector2(rect.X + 10, rect.Y + 10), Color.Black);
        // }
        
        private void DrawInventory(SpriteBatch spriteBatch)
        {
            int yOffset = 750;
            spriteBatch.DrawString(_font, "Inventory:", new Vector2(50, yOffset), Color.Black);
            
            var fishList = _game.Inventory.GetAllFish();
            if (fishList.Count > 0)
            {
                var lastFish = fishList[fishList.Count - 1];
                spriteBatch.DrawString(_font, $"Last fish: {lastFish.Name} ({lastFish.Rarity}) - {lastFish.Weight:F1}kg", 
                    new Vector2(50, yOffset + 30), Color.Black);
                spriteBatch.DrawString(_font, $"Total fish: {_game.Inventory.TotalFish}", 
                    new Vector2(50, yOffset + 60), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(_font, "No fish yet!", 
                    new Vector2(50, yOffset + 30), Color.Gray);
            }
        }
        
        private void DrawFishCollection(SpriteBatch spriteBatch)
        {
            int yOffset = 900;
            spriteBatch.DrawString(_font, "Collection by Rarity:", new Vector2(50, yOffset), Color.Black);
            
            int xOffset = 50;
            foreach (FishRarity rarity in System.Enum.GetValues(typeof(FishRarity)))
            {
                int count = _game.Inventory.GetFishByRarity(rarity).Count;
                Color rarityColor = GetRarityColor(rarity);
                spriteBatch.DrawString(_font, $"{rarity}: {count}", 
                    new Vector2(xOffset, yOffset + 30), rarityColor);
                xOffset += 120;
            }
        }
        
        private Color GetRarityColor(FishRarity rarity)
        {
            switch (rarity)
            {
                case FishRarity.Common: return Color.Gray;
                case FishRarity.Uncommon: return Color.Green;
                case FishRarity.Rare: return Color.Blue;
                case FishRarity.Epic: return Color.Purple;
                case FishRarity.Legendary: return Color.Gold;
                default: return Color.White;
            }
        }
    }
}