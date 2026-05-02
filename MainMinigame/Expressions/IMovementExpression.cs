using System;
using Microsoft.Xna.Framework;

namespace FishingGame.FishingMiniGame.Expressions
{
    public interface IMovementExpression
    {
        float Interpret(float time, float deltaTime, float currentY, float minY, float maxY);
        IMovementExpression Clone();
    }
}