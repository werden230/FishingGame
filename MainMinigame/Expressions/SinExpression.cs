using System;

namespace FishingGame.FishingMiniGame.Expressions
{
    public class SinExpression : IMovementExpression
    {
        private readonly float _frequency;
        private readonly float _amplitude;
        
        public SinExpression(float frequency, float amplitude)
        {
            _frequency = frequency;
            _amplitude = amplitude;
        }
        
        public float Interpret(float time, float deltaTime, float currentY, float minY, float maxY)
        {
            return (float)(Math.Sin(time * _frequency) * _amplitude);
        }
        
        public IMovementExpression Clone()
        {
            return new SinExpression(_frequency, _amplitude);
        }
    }
}