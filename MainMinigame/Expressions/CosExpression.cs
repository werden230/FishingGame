using System;

namespace FishingGame.FishingMiniGame.Expressions
{
    public class CosExpression : IMovementExpression
    {
        private readonly float _frequency;
        private readonly float _amplitude;
        
        public CosExpression(float frequency, float amplitude)
        {
            _frequency = frequency;
            _amplitude = amplitude;
        }
        
        public float Interpret(float time, float deltaTime, float currentY, float minY, float maxY)
        {
            return (float)(Math.Cos(time * _frequency) * _amplitude);
        }
        
        public IMovementExpression Clone()
        {
            return new CosExpression(_frequency, _amplitude);
        }
    }
}