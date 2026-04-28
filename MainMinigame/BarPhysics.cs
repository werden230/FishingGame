using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace FishingMiniGame.Physics
{
    public class BarPhysics
    {
        public float Velocity { get; private set; }
        public float TargetVelocity { get; private set; }
        public float Position { get; private set; }
        
        private float damping;
        private float gravity;
        private float minPosition;
        private float maxPosition;
        
        public BarPhysics(float startPosition, float minY, float maxY, float damping = 0.95f, float gravity = 1.2f)
        {
            Position = startPosition;
            Velocity = 0;
            TargetVelocity = 0;
            this.damping = damping;
            this.gravity = gravity;
            this.minPosition = minY;
            this.maxPosition = maxY;
        }
        
        public void ApplyClick(float force)
        {
            TargetVelocity = force;
        }
        
        public void ApplyHold(float acceleration, float holdTime)
        {
            float accel = acceleration * MathHelper.Clamp(holdTime * 1.5f, 0.5f, 2.5f);
            TargetVelocity += accel;
            TargetVelocity = MathHelper.Max(TargetVelocity, -15f);
        }
        
        public void ApplyGravity()
        {
            TargetVelocity += gravity;
            TargetVelocity = MathHelper.Min(TargetVelocity, 12f);
        }
        
        public void Update(bool isInputActive)
        {
            // Плавное изменение скорости
            Velocity = Velocity * damping + TargetVelocity * (1 - damping);
            
            // Обновление позиции
            Position += Velocity;
            
            // Ограничение границами
            if (Position < minPosition)
            {
                Position = minPosition;
                if (Velocity < 0) Velocity = 0;
            }
            
            if (Position > maxPosition)
            {
                Position = maxPosition;
                if (Velocity > 0) Velocity = 0;
            }
            
            // Затухание при отсутствии ввода
            if (!isInputActive)
            {
                TargetVelocity *= 0.98f;
            }
        }
        
        public void ResetTargetVelocity()
        {
            TargetVelocity = 0;
        }
    }
}