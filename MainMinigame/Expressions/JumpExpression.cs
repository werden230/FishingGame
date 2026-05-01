using System;

namespace FishingGame.FishingMiniGame.Expressions
{
    public class JumpExpression : IMovementExpression
    {
        private readonly float _interval;
        private readonly float _strength;
        private float _lastJumpTime;
        private bool _isJumping;
        private float _jumpProgress;
        
        public JumpExpression(float interval, float strength)
        {
            _interval = interval;
            _strength = strength;
            _lastJumpTime = -interval;
            _isJumping = false;
            _jumpProgress = 0;
        }
        
        public float Interpret(float time, float deltaTime, float currentY, float minY, float maxY)
        {
            float result = 0;
            
            if (!_isJumping && time - _lastJumpTime >= _interval)
            {
                _isJumping = true;
                _jumpProgress = 0;
                _lastJumpTime = time;
            }
            
            if (_isJumping)
            {
                _jumpProgress += deltaTime;
                float jumpDuration = 0.5f;
                
                if (_jumpProgress <= jumpDuration)
                {
                    // Параболический прыжок
                    float t = _jumpProgress / jumpDuration;
                    result = (float)(Math.Sin(t * Math.PI) * _strength);
                }
                else
                {
                    _isJumping = false;
                }
            }
            
            return result;
        }
        
        public IMovementExpression Clone()
        {
            return new JumpExpression(_interval, _strength);
        }
    }
}