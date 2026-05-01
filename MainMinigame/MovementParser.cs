using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FishingGame.FishingMiniGame.Expressions;

namespace FishingGame.FishingMiniGame
{
    public class MovementParser
    {
        public IMovementExpression Parse(string expression)
        {
            var composite = new CompositeExpression();
            var patterns = new Dictionary<string, Func<Match, IMovementExpression>>
            {
                { @"SIN\(([\d.]+),\s*([\d.]+)\)", match => 
                    new SinExpression(float.Parse(match.Groups[1].Value), float.Parse(match.Groups[2].Value)) },
                
                { @"COS\(([\d.]+),\s*([\d.]+)\)", match => 
                    new CosExpression(float.Parse(match.Groups[1].Value), float.Parse(match.Groups[2].Value)) },
                
                { @"JUMP\(([\d.]+),\s*([\d.]+)\)", match => 
                    new JumpExpression(float.Parse(match.Groups[1].Value), float.Parse(match.Groups[2].Value)) },
                
                { @"RANDOM\(([\d.]+),\s*([\d.]+)\)", match => 
                    new RandomExpression(float.Parse(match.Groups[1].Value), float.Parse(match.Groups[2].Value)) }
            };
            
            foreach (var pattern in patterns)
            {
                var matches = Regex.Matches(expression, pattern.Key);
                foreach (Match match in matches)
                {
                    composite.AddExpression(pattern.Value(match));
                }
            }
            
            return composite;
        }
    }
}