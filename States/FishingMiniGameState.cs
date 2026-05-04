using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.MiniGames;
using FishingGame.FishSystem;
using System;
using System.Reflection.Metadata.Ecma335;

namespace FishingGame
{
    public class FishingMiniGameState : GameState
    {
        private Game1 _game;
        private Texture2D _playerTexture;
        private FishingMiniGameLogic _fishingMiniGame;
        private Biome _currentBiome;
        private Vector2 _position = new Vector2(400, 350);
        private Fish _rewardFish;
        private float _rewardDisplayTimer;
        private bool _showReward;
        
        public FishingMiniGameState(Game1 game, Biome biome)
        {
            _game = game;
            _currentBiome = biome;
            _rewardFish = _currentBiome.GetRandomFish();
        }
        
        public override void Enter()
        {
            // Загружаем ресурсы для мини-игры
            Texture2D fishingBar = _game.Content.Load<Texture2D>("Fishing_Bar");
            Texture2D fishingFish = _game.Content.Load<Texture2D>("Fishing_Fish");
            Texture2D fishingBGTexture = _game.Content.Load<Texture2D>("Fishing_BG");
            _playerTexture = _game.Content.Load<Texture2D>("player");

            _fishingMiniGame = new FishingMiniGameLogic(
                _position,
                fishingBar,
                fishingFish,
                fishingBGTexture,
                _game.WhiteTexture,
                _rewardFish.MovementPattern
            );
            
            _showReward = false;
            _rewardDisplayTimer = 0;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (_showReward)
            {
                _rewardDisplayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_rewardDisplayTimer <= 0)
                {
                    _game.ChangeState(new MainGameState(_game, _currentBiome.BiomeType));
                }
                return;
            }
            
            _fishingMiniGame?.Update(gameTime);
            
            // Проверяем завершение мини-игры
            if (_fishingMiniGame != null && !_fishingMiniGame.IsGameActive)
            {
                // Получаем награду в зависимости от результата
                if (_fishingMiniGame.DidPlayerWin)
                {
                    _game.Inventory.AddFish(_rewardFish);
                    _showReward = true;
                    _rewardDisplayTimer = 3.0f; // Показываем награду 3 секунды
                }
                else
                {
                    // При поражении возвращаемся без награды
                    _game.ChangeState(new MainGameState(_game));
                }
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentBiome.BackgroundTexture, new Rectangle(0, 0, 720,  1280), Color.White);
            spriteBatch.Draw(_playerTexture, new Vector2((720-_playerTexture.Width)/2, 640-_playerTexture.Height+30), Color.White);

            if (_showReward)
            {
                Vector2 position = new Vector2(260, 410);
                string fishName = _rewardFish.Name;
                _rewardFish.Texture = _game.Content.Load<Texture2D>($"fish/{_rewardFish.Name}");
                DrawReward(spriteBatch, position, fishName, _rewardFish.Texture);
            }
            else
            {
                _fishingMiniGame?.Draw(spriteBatch);
            }
        }

        private void DrawReward(SpriteBatch spriteBatch, Vector2 position, string fishName, Texture2D fishTexture)
        {
            SpriteFont font = _game.Content.Load<SpriteFont>("DefaultFont");
            Texture2D _rewardBlobTexture = _game.Content.Load<Texture2D>("fishing_blob");
            int sizeX = (int)(_rewardBlobTexture.Width/1.5);
            int sizeY = (int)(_rewardBlobTexture.Height/1.5);
            Vector2 textSize = font.MeasureString(fishName);
            spriteBatch.Draw(_rewardBlobTexture, new Rectangle((int)position.X, (int)position.Y, sizeX, sizeY), Color.White);
            
            spriteBatch.DrawString(font, fishName,
                    new Vector2(position.X + (sizeX-textSize.X)/2, position.Y+10),
                    Color.Black);

            spriteBatch.Draw(_rewardFish.Texture, new Rectangle((int)position.X+21, (int)position.Y+sizeY-85, 48,  48), Color.White);

        }
        
        public override void Exit()
        {
            _fishingMiniGame = null;
        }
    }
}