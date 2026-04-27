using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FishingMiniGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Sprite fishingBGSprite;
        private Sprite fishingBarSprite;  // green zone
        private Sprite fishingFishSprite;
        private Texture2D _whiteTexture;

        // Размеры окна
        private const int WindowWidth = 720;
        private const int WindowHeight = 1280;

        private Vector2 fishingMiniGamePosition = new Vector2(100, 50);
        
        // Правая полоса прогресса
        private Rectangle progressBarBg;
        private Rectangle progressBarFill;
        private float progress = 0.5f;
        
        // Позиция и физика зеленой полоски
        private Vector2 barPosition;
        private float barVelocity = 0f;
        private float targetVelocity = 0f;
        
        // Параметры физики
        private const float ClickForce = -2.5f;
        private const float HoldAcceleration = -0.3f;
        private const float Gravity = 1.2f;
        private const float Damping = 0.95f;
        
        // Границы движения
        private float minBarY;
        private float maxBarY;
        private float barHeight = 108f;
        
        // Управление
        private MouseState previousMouseState;
        private bool isMouseHeld = false;
        private float holdTime = 0f;
        
        // Рыбка
        private Vector2 fishPosition;
        private float fishY;
        private float fishVelocity = 0f;
        private Random random = new Random();

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
            float startY = fishingMiniGamePosition.Y + 150 + (437f / 2) - (barHeight / 2);
            barPosition = new Vector2(fishingMiniGamePosition.X + 81, startY);
            
            // Определяем границы движения
            minBarY = fishingMiniGamePosition.Y + 20;
            maxBarY = fishingMiniGamePosition.Y + 449 - barHeight;
            
            // Начальная позиция рыбки
            fishY = minBarY + random.Next(0, (int)(maxBarY - minBarY));
            fishPosition = new Vector2(
                fishingMiniGamePosition.X + 81,
                fishY
            );
            
            // Настройка полосы прогресса
            const int progressBarWidth = 12;
            const int progressBarHeight = 437;
            int progressBarX = (int)fishingMiniGamePosition.X + 126;
            int progressBarY = (int)fishingMiniGamePosition.Y + 15;
    
            progressBarBg = new Rectangle(
                progressBarX,
                progressBarY,
                progressBarWidth,
                progressBarHeight
            );
            
            UpdateProgressBar();
            
            previousMouseState = Mouse.GetState();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Texture2D fishingBGTexture = Content.Load<Texture2D>("Fishing_BG");
            Texture2D fishingBar = Content.Load<Texture2D>("Fishing_Bar");
            Texture2D fishingFish = Content.Load<Texture2D>("Fishing_Fish");
            
            _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });

            fishingBGSprite = new Sprite(fishingBGTexture, fishingMiniGamePosition);
            fishingBarSprite = new Sprite(fishingBar, barPosition);
            fishingFishSprite = new Sprite(fishingFish, fishPosition);
        }

        private void HandleInput()
        {
            MouseState currentMouseState = Mouse.GetState();
            bool isLeftButtonPressed = currentMouseState.LeftButton == ButtonState.Pressed;
            bool wasLeftButtonPressed = previousMouseState.LeftButton == ButtonState.Pressed;
            
            // Обработка кликов
            if (isLeftButtonPressed && !wasLeftButtonPressed)
            {
                targetVelocity = ClickForce;
                holdTime = 0f;
                isMouseHeld = false;
            }
            
            // Обработка удержания
            if (isLeftButtonPressed && wasLeftButtonPressed)
            {
                if (!isMouseHeld)
                {
                    isMouseHeld = true;
                    holdTime = 0f;
                }
                
                holdTime += 0.016f;
                float acceleration = HoldAcceleration * MathHelper.Clamp(holdTime * 1.5f, 0.5f, 2.5f);
                targetVelocity += acceleration;
                targetVelocity = MathHelper.Max(targetVelocity, -15f);
            }
            
            // Если кнопка отпущена
            if (!isLeftButtonPressed)
            {
                isMouseHeld = false;
                targetVelocity += Gravity;
                targetVelocity = MathHelper.Min(targetVelocity, 12f);
            }
            
            // Трение при отсутствии ввода
            if (!isLeftButtonPressed && Math.Abs(targetVelocity) < 0.5f)
            {
                targetVelocity = Gravity * 0.5f;
            }
            
            previousMouseState = currentMouseState;
        }
        
        private void UpdateBarPhysics()
        {
            // Плавное изменение скорости
            barVelocity = barVelocity * Damping + targetVelocity * (1 - Damping);
            
            // Обновление позиции
            barPosition.Y += barVelocity;
            
            // Ограничение границами
            if (barPosition.Y < minBarY)
            {
                barPosition.Y = minBarY;
                if (barVelocity < 0) barVelocity = 0;
            }
            
            if (barPosition.Y > maxBarY)
            {
                barPosition.Y = maxBarY;
                if (barVelocity > 0) barVelocity = 0;
            }
            
            // Затухание целевой скорости
            if (!isMouseHeld && previousMouseState.LeftButton == ButtonState.Released)
            {
                targetVelocity *= 0.98f;
            }
            
            if (fishingBarSprite != null)
                fishingBarSprite.position = barPosition;
        }
        
        private void UpdateFishMovement()
        {
            // Случайное движение рыбки
            fishVelocity += (float)(random.NextDouble() - 0.5) * 0.5f;
            fishVelocity = MathHelper.Clamp(fishVelocity, -3f, 3f);
            
            fishY += fishVelocity;
            
            // Ограничение границами
            if (fishY < minBarY)
            {
                fishY = minBarY;
                fishVelocity = Math.Abs(fishVelocity) * 0.5f;
            }
            
            if (fishY > maxBarY)
            {
                fishY = maxBarY;
                fishVelocity = -Math.Abs(fishVelocity) * 0.5f;
            }
            
            fishPosition.Y = fishY;
            if (fishingFishSprite != null)
                fishingFishSprite.position = fishPosition;
        }
        
        private void UpdateProgressLogic()
        {
            // Проверка, находится ли рыбка внутри зеленой полоски
            bool isFishInside = fishY >= barPosition.Y && 
                               fishY <= barPosition.Y + barHeight;
            
            if (isFishInside)
            {
                progress += 0.01f;
                if (progress > 1f) progress = 1f;
            }
            else
            {
                progress -= 0.008f;
                if (progress < 0f) progress = 0f;
            }
            
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            int fillHeight = (int)(progressBarBg.Height * progress);
            progressBarFill = new Rectangle(
                progressBarBg.X,
                progressBarBg.Y + progressBarBg.Height - fillHeight,
                progressBarBg.Width,
                fillHeight
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            HandleInput();
            UpdateBarPhysics();
            UpdateFishMovement();
            UpdateProgressLogic();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(fishingBGSprite.texture, fishingBGSprite.position, Color.White);
            _spriteBatch.Draw(fishingBarSprite.texture, fishingBarSprite.position, Color.White);
            _spriteBatch.Draw(fishingFishSprite.texture, fishingFishSprite.position, Color.White);
            _spriteBatch.Draw(_whiteTexture, progressBarFill, Color.Lime);
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}