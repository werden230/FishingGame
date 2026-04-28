using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame;
using FishingMiniGame.Entities;
using FishingMiniGame.Physics;
using FishingMiniGame.Utilities;

namespace FishingMiniGame.MiniGames
{
    public class FishingMiniGameLogic
    {
        private FishingBG fishingBG;
        private FishingBar fishingBar;
        private Fish fish;
        private ProgressBar progressBar;
        private BarPhysics barPhysics;
        private InputHelper input;
        
        private Vector2 gamePosition;
        private float minY;
        private float maxY;
        
        // Параметры физики
        private const float ClickForce = -2.5f;
        private const float HoldAcceleration = -0.3f;
        
        // Параметры прогресса
        private const float ProgressGainRate = 0.005f;
        private const float ProgressLossRate = 0.008f;
        
        public bool IsGameActive { get; private set; } = true;

        public bool DidPlayerWin { get; private set; } = false;
        
        public FishingMiniGameLogic(Vector2 position, Texture2D barTexture, Texture2D fishTexture, Texture2D fishingBGTexture, Texture2D whiteTexture)
        {
            gamePosition = position;
            
            // Определяем границы движения
            float barHeight = barTexture.Height;
            minY = position.Y + 20;
            maxY = position.Y + 449 - barHeight;
            
            // Начальная позиция
            float startY = maxY;
            Vector2 barStartPosition = new Vector2(position.X + 81, startY);
            
            // Создаём сущности
            fishingBG = new FishingBG(fishingBGTexture, position);

            fishingBar = new FishingBar(barTexture, barStartPosition, minY, maxY);
            
            Vector2 fishStartPosition = new Vector2(position.X + 81, startY);
            fish = new Fish(fishTexture, fishStartPosition, minY, maxY);
            
            // Настройка прогресс-бара
            Rectangle progressBarRect = new Rectangle(
                (int)position.X + 126,
                (int)position.Y + 15,
                12,
                437
            );
            progressBar = new ProgressBar(whiteTexture, progressBarRect);
            
            // Физика
            barPhysics = new BarPhysics(startY, minY, maxY);
            
            // Ввод
            input = new InputHelper();
        }
        
        public void Update(GameTime gameTime)
        {
            if (!IsGameActive) return;
            
            input.Update();
            
            // Обработка ввода
            if (input.IsLeftButtonPressed)
            {
                barPhysics.ApplyClick(ClickForce);
            }
            else if (input.IsLeftButtonHeld)
            {
                barPhysics.ApplyHold(HoldAcceleration, input.HoldTime);
            }
            else
            {
                barPhysics.ApplyGravity();
            }
            
            // Обновление физики
            barPhysics.Update(input.IsLeftButtonHeld);
            
            // Обновление позиции полоски
            fishingBar.UpdatePosition(barPhysics.Position);
            
            // Обновление рыбки
            fish.Update();
            
            // Обновление прогресса
            if (fishingBar.Contains(fish.Y))
            {
                progressBar.Increase(ProgressGainRate);
            }
            else
            {
                progressBar.Decrease(ProgressLossRate);
            }
            
            // Проверка условий победы/поражения
            if (progressBar.IsComplete())
            {
                IsGameActive = false;
                DidPlayerWin = true;
                // Победа!
            }
            else if (progressBar.IsEmpty())
            {
                IsGameActive = false;
                DidPlayerWin = false;
                // Поражение - рыба сорвалась
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            fishingBG.Draw(spriteBatch);
            fishingBar.Draw(spriteBatch);
            fish.Draw(spriteBatch);
            progressBar.Draw(spriteBatch);
        }
        
        public void Reset()
        {
            IsGameActive = true;
            float startY = minY + (maxY - minY) / 2;
            barPhysics = new BarPhysics(startY, minY, maxY);
            fishingBar.UpdatePosition(barPhysics.Position);
            
            // Сброс прогресса
            Rectangle progressBarRect = new Rectangle(
                (int)gamePosition.X + 126,
                (int)gamePosition.Y + 15,
                12,
                437
            );
            progressBar = new ProgressBar(progressBar.GetType().GetField("whiteTexture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(progressBar) as Texture2D, progressBarRect);
        }
    }
}