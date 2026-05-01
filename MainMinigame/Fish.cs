using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingGame.FishingMiniGame;
using FishingGame.FishingMiniGame.Expressions;
using FishingGame.FishSystem;
using System;

namespace FishingMiniGame.Entities
{
    public class FishEntitie
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; private set; }
        public float Y => Position.Y;
        
        private IMovementExpression _movementExpression;
        private float _time;
        private float _baseY;
        private float _offsetY;
        private float _minY;
        private float _maxY;
        private float _velocityY;
        
        public FishEntitie(Texture2D texture, Vector2 startPosition, float minY, float maxY, string movingPattern)
        {
            Texture = texture;
            Position = startPosition;
            _minY = minY;
            _maxY = maxY;
            _time = 0;
            _baseY = (minY + maxY) / 2;
            _offsetY = 0;
            _velocityY = 0;
            
            var parser = new MovementParser();
            _movementExpression = parser.Parse(movingPattern);
        }
        
        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _time += deltaTime;
            
            float targetOffset = _movementExpression.Interpret(_time, deltaTime, Position.Y, _minY, _maxY);
            
            _offsetY = MathHelper.Lerp(_offsetY, targetOffset, 0.1f);
            
            float newY = _baseY + _offsetY;
            
            if (newY < _minY)
            {
                newY = _minY;
                _offsetY = _minY - _baseY;
                _velocityY = Math.Abs(_velocityY) * 0.5f;
            }
            
            if (newY > _maxY)
            {
                newY = _maxY;
                _offsetY = _maxY - _baseY;
                _velocityY = -Math.Abs(_velocityY) * 0.5f;
            }
            
            Position = new Vector2(Position.X, newY);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}