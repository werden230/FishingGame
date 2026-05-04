using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.Entities;
using FishingMiniGame.Physics;
using FishingMiniGame.Observer;
using FishingMiniGame.Utilities;

namespace FishingMiniGame.MiniGames
{
    public class FishingMiniGameLogic
    {
        private FishingBG fishingBG;
        private FishingBar fishingBar;
        private FishEntitie fish;
        private ProgressBar progressBar;
        private BarPhysics barPhysics;
        private InputHelper input;
        
        private Vector2 gamePosition;
        private float minY;
        private float maxY;
        private bool wasContact;
        
        // Параметры физики
        private const float ClickForce = -2.5f;
        private const float HoldAcceleration = -0.3f;
        
        public bool IsGameActive { get; private set; } = true;
        public bool DidPlayerWin { get; private set; } = false;
        
        public FishingMiniGameLogic(Vector2 position, Texture2D barTexture, Texture2D fishTexture, 
            Texture2D fishingBGTexture, Texture2D whiteTexture, string movingPattern)
        {
            gamePosition = position;
            
            // Определяем границы движения
            float barHeight = barTexture.Height;
            minY = position.Y + 20;
            maxY = position.Y + 449 - barHeight;
            
            // Начальная позиция
            float startY = maxY/2;
            Vector2 barStartPosition = new Vector2(position.X + 81, startY);
            
            // Создаём сущности
            fishingBG = new FishingBG(fishingBGTexture, position);
            fishingBar = new FishingBar(barTexture, barStartPosition, minY, maxY);
            
            Vector2 fishStartPosition = new Vector2(position.X + 81, startY);
            fish = new FishEntitie(fishTexture, fishStartPosition, minY, maxY, movingPattern);
            
            // Настройка прогресс-бара
            Rectangle progressBarRect = new Rectangle(
                (int)position.X + 126,
                (int)position.Y + 15,
                12,
                437
            );
            progressBar = new ProgressBar(whiteTexture, progressBarRect);
            
            // Подписываем ProgressBar на события
            fishingBar.Attach(progressBar);
            fish.Attach(progressBar);
            
            // Физика
            barPhysics = new BarPhysics(startY, minY, maxY);
            
            // Ввод
            input = new InputHelper();
            
            wasContact = false;
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
            fish.Update(gameTime);
            
            // Проверка контакта между полоской и рыбкой
            bool isContact = fishingBar.Contains(fish.Y);
            progressBar.UpdateContact(isContact);

            if (isContact && !wasContact)
            {
                // Начало контакта
                fishingBar.Notify(GameEvent.BarFishContact);
                fish.Notify(GameEvent.BarFishContact);
            }
            else if (isContact && wasContact)
            {
                // Продолжение контакта - каждый кадр
                fishingBar.Notify(GameEvent.BarFishContactUpdate);
                fish.Notify(GameEvent.BarFishContactUpdate);
            }
            else if (!isContact && wasContact)
            {
                // Потеря контакта
                fishingBar.Notify(GameEvent.BarFishLost);
                fish.Notify(GameEvent.BarFishLost);
            }

            wasContact = isContact;
            
            // Проверка условий победы/поражения
            if (progressBar.IsComplete())
            {
                IsGameActive = false;
                DidPlayerWin = true;
                fishingBar.Notify(GameEvent.GameWin);
                fish.Notify(GameEvent.GameWin);
            }
            else if (progressBar.IsEmpty())
            {
                IsGameActive = false;
                DidPlayerWin = false;
                fishingBar.Notify(GameEvent.GameLose);
                fish.Notify(GameEvent.GameLose);
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
            DidPlayerWin = false;
            wasContact = false;
            
            float startY = minY + (maxY - minY) / 2;
            barPhysics = new BarPhysics(startY, minY, maxY);
            fishingBar.UpdatePosition(barPhysics.Position);
            
            // Сброс прогресса
            progressBar.Reset();
        }
    }
}