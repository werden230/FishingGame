using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FishingGame.FishSystem;

namespace FishingGame
{
    public class MainGameState : GameState
    {
        private readonly Game1 _game;
        private List<Biome> _biomes;
        private int _currentBiomeIndex;
        private SpriteFont _font;
        private Texture2D _inventoryTexture;

        private bool _isMousePressed;
        private bool _isInventoryOpen;
        private bool _wasInventoryTogglePressed;

        private readonly Rectangle _nextButton;
        private readonly Rectangle _prevButton;
        private readonly Rectangle _sellAllButton;

        private const int InventoryColumns = 12;
        private const int InventoryRows = 3;
        private static readonly Point InventoryPanelSize = new Point(580, 200);
        private static readonly Point InventoryPanelPosition = new Point(20, 820);
        private static readonly Point InventoryInnerOrigin = new Point(20, 32);
        private const int InventorySlotSize = 49;
        private const int InventorySlotHorizontalSpacing = -4;
        private const int InventorySlotVerticalSpacing = 0;

        public MainGameState(Game1 game)
        {
            _game = game;
            _currentBiomeIndex = 0;

            _nextButton = new Rectangle(470, 600, 180, 50);
            _prevButton = new Rectangle(50, 600, 180, 50);
            _sellAllButton = new Rectangle(500, 1060, 150, 50);
        }

        public override void Enter()
        {
            _font = _game.Content.Load<SpriteFont>("DefaultFont");
            _inventoryTexture = _game.Content.Load<Texture2D>("Inventory");
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

            bool inventoryTogglePressed = keyState.IsKeyDown(Keys.I);
            if (inventoryTogglePressed && !_wasInventoryTogglePressed)
            {
                _isInventoryOpen = !_isInventoryOpen;
            }
            _wasInventoryTogglePressed = inventoryTogglePressed;

            if (!_isMousePressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                _isMousePressed = true;
                Point mousePos = new Point(mouseState.X, mouseState.Y);

                if (_nextButton.Contains(mousePos))
                {
                    _currentBiomeIndex = (_currentBiomeIndex + 1) % _biomes.Count;
                }
                else if (_prevButton.Contains(mousePos))
                {
                    _currentBiomeIndex = _currentBiomeIndex == 0 ? _biomes.Count - 1 : _currentBiomeIndex - 1;
                }
                else if (_isInventoryOpen && _sellAllButton.Contains(mousePos))
                {
                    SellAllFish();
                }
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                _isMousePressed = false;
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
            spriteBatch.Draw(currentBiome.BackgroundTexture, new Rectangle(0, 0, 720, 1280), Color.White);

            DrawButton(spriteBatch, _nextButton, "Next");
            DrawButton(spriteBatch, _prevButton, "Previous");

            spriteBatch.DrawString(_font, "Press SPACE to fish", new Vector2(255, 700), Color.Black);
            spriteBatch.DrawString(_font, "Press I to open inventory", new Vector2(220, 740), Color.Black);
            spriteBatch.DrawString(_font, $"Money: {_game.Money}g", new Vector2(50, 50), Color.DarkGreen);

            if (_isInventoryOpen)
            {
                DrawInventory(spriteBatch);
            }
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle rect, string text)
        {
            spriteBatch.Draw(_game.WhiteTexture, rect, Color.Gold);
            spriteBatch.DrawString(_font, text, new Vector2(rect.X + 10, rect.Y + 10), Color.Black);
        }

        private void DrawInventory(SpriteBatch spriteBatch)
        {
            var panelRect = new Rectangle(
                InventoryPanelPosition.X,
                InventoryPanelPosition.Y,
                InventoryPanelSize.X,
                InventoryPanelSize.Y);

            spriteBatch.Draw(_inventoryTexture, panelRect, Color.White);
            spriteBatch.DrawString(_font, $"Fish: {_game.Inventory.TotalFish}", new Vector2(50, 1068), Color.Black);
            spriteBatch.DrawString(_font, $"Value: {_game.Inventory.GetTotalPrice()}g", new Vector2(220, 1068), Color.Black);
            DrawButton(spriteBatch, _sellAllButton, "Sell all");
            DrawInventoryGrid(spriteBatch, panelRect);
        }

        private void DrawInventoryGrid(SpriteBatch spriteBatch, Rectangle panelRect)
        {
            IReadOnlyList<FishStack> stacks = _game.Inventory.Stacks;
            int totalSlots = InventoryColumns * InventoryRows;

            for (int i = 0; i < totalSlots; i++)
            {
                Rectangle slotRect = GetSlotRect(panelRect, i);

                if (i >= stacks.Count)
                {
                    continue;
                }

                FishStack stack = stacks[i];
                Rectangle itemRect = new Rectangle(
                    slotRect.X + 8,
                    slotRect.Y + 8,
                    slotRect.Width - 16,
                    slotRect.Height - 16);

                spriteBatch.Draw(_game.WhiteTexture, itemRect, Color.White);

                string countText = $"x{stack.GetFishCount()}";
                Vector2 countSize = _font.MeasureString(countText);
                Vector2 countPosition = new Vector2(
                    slotRect.Right - countSize.X - 4,
                    slotRect.Bottom - countSize.Y - 2);

                spriteBatch.DrawString(_font, countText, countPosition + new Vector2(1, 1), Color.Black);
                spriteBatch.DrawString(_font, countText, countPosition, Color.Black);
            }
        }

        private Rectangle GetSlotRect(Rectangle panelRect, int slotIndex)
        {
            int column = slotIndex % InventoryColumns;
            int row = slotIndex / InventoryColumns;

            return new Rectangle(
                panelRect.X + InventoryInnerOrigin.X + column * (InventorySlotSize + InventorySlotHorizontalSpacing),
                panelRect.Y + InventoryInnerOrigin.Y + row * (InventorySlotSize + InventorySlotVerticalSpacing),
                InventorySlotSize,
                InventorySlotSize);
        }

        private void SellAllFish()
        {
            int totalPrice = _game.Inventory.SellAll();
            if (totalPrice > 0)
            {
                _game.AddMoney(totalPrice);
            }
        }
    }
}