using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.MiniGames;
using FishingGame.FishSystem;

namespace FishingGame
{
    public class FishingMiniGameState : GameState
    {
        private Game1 _game;
        private FishingMiniGameLogic _fishingMiniGame;
        private Biome _currentBiome;
        private Vector2 _position = new Vector2(250, 250);
        private Fish _rewardFish;
        private float _rewardDisplayTimer;
        private bool _showReward;
        
        public FishingMiniGameState(Game1 game, Biome biome)
        {
            _game = game;
            _currentBiome = biome;
        }
        
        public override void Enter()
        {
            // Загружаем ресурсы для мини-игры
            Texture2D fishingBar = _game.Content.Load<Texture2D>("Fishing_Bar");
            Texture2D fishingFish = _game.Content.Load<Texture2D>("Fishing_Fish");
            Texture2D fishingBGTexture = _game.Content.Load<Texture2D>("Fishing_BG");

            _fishingMiniGame = new FishingMiniGameLogic(
                _position,
                fishingBar,
                fishingFish,
                fishingBGTexture,
                _game.WhiteTexture
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
                    _rewardFish = _currentBiome.GetRandomFish();
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
            if (_showReward)
            {
                // Рисуем экран награды
                // spriteBatch.Draw(_game.WhiteTexture, 
                //     new Rectangle(0, 0, 720, 1280), 
                //     new Color(0, 0, 0, 200));
                
                SpriteFont font = _game.Content.Load<SpriteFont>("DefaultFont");
                string rewardText = $"You caught a {_rewardFish.Name}! ({_rewardFish.Rarity})";
                Vector2 textSize = font.MeasureString(rewardText);
                
                spriteBatch.DrawString(font, rewardText,
                    new Vector2(360 - textSize.X / 2, 640 - textSize.Y / 2),
                    Color.Gold);
                
                spriteBatch.DrawString(font, $"Value: ${_rewardFish.Price}",
                    new Vector2(360 - 50, 680),
                    Color.White);
            }
            else
            {
                _fishingMiniGame?.Draw(spriteBatch);
            }
        }
        
        public override void Exit()
        {
            _fishingMiniGame = null;
        }
    }
}