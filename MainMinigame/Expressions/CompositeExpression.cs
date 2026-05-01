using System.Collections.Generic;

namespace FishingGame.FishingMiniGame.Expressions
{
    public class CompositeExpression : IMovementExpression
    {
        private readonly List<IMovementExpression> _expressions;
        public int ExpressionCount => _expressions.Count;
        
        public CompositeExpression()
        {
            _expressions = new List<IMovementExpression>();
        }
        
        public void AddExpression(IMovementExpression expression)
        {
            _expressions.Add(expression);
        }
        
        public float Interpret(float time, float deltaTime, float currentY, float minY, float maxY)
        {
            float result = 0;
            foreach (var expression in _expressions)
            {
                result += expression.Interpret(time, deltaTime, currentY, minY, maxY);
            }
            return result;
        }
        
        public IMovementExpression Clone()
        {
            var clone = new CompositeExpression();
            foreach (var expression in _expressions)
            {
                clone.AddExpression(expression.Clone());
            }
            return clone;
        }
    }
}