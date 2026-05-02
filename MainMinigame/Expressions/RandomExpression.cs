using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FishingGame.FishingMiniGame.Expressions
{
    public class RandomExpression : IMovementExpression
    {
        private readonly float _intensity;
        private readonly float _smoothness;
        private Random _random;
        private float _currentValue;
        private float _targetValue;
        private float _changeTimer;
        
        public RandomExpression(float intensity, float smoothness)
        {
            _intensity = intensity;
            _smoothness = smoothness;
            _random = new Random();
            _currentValue = 0;
            _targetValue = 0;
            _changeTimer = 0;
        }
        
        public float Interpret(float time, float deltaTime, float currentY, float minY, float maxY)
        {
            _changeTimer += deltaTime;
            
            // Меняем направление с определенной частотой (основано на smoothness)
            if (_changeTimer >= _smoothness)
            {
                _changeTimer = 0;
                _targetValue = (float)((_random.NextDouble() * 2 - 1) * _intensity);
            }
            
            // Плавно двигаемся к целевой позиции
            _currentValue = MathHelper.Lerp(_currentValue, _targetValue, 0.1f);
            
            return _currentValue;
        }
        
        public IMovementExpression Clone()
        {
            return new RandomExpression(_intensity, _smoothness);
        }
    }
}