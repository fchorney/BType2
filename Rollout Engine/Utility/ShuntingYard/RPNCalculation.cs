using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rollout.Utility.ShuntingYard
{
    /// <summary>
    /// Reverse Polish Notation Calculation
    /// </summary>
    public class RPNCalculation
    {
        private static readonly Random Rand = new Random();
        private readonly List<EToken> tokens;

        public RPNCalculation(List<EToken> tokens)
        {
            this.tokens = tokens;
        }

        private EToken Solve()
        {
            var stack = new Stack<EToken>();

            foreach (var token in tokens)
            {
                if (token.IsNumber || token.IsFunction)
                {
                    stack.Push(token);
                }
                else if (token.IsOperator)
                {
                    var second = stack.Pop();
                    var first = stack.Pop();
                    stack.Push(Operate(first, second, token));
                }
            }
            return stack.Pop();
        }

        private EToken Operate(EToken first, EToken second, EToken op)
        {
            var a = doubleOrFunction(first.Value);
            var b = doubleOrFunction(second.Value);
            double result;
            switch (op.Value)
            {
                case "+":
                    result = (a + b);
                    break;
                case "-":
                    result = (a - b);
                    break;
                case "*":
                    result = (a * b);
                    break;
                case "/":
                    result = (a / b);
                    break;
                case "%":
                    result = (a % b);
                    break;
                case "^":
                    result = Math.Pow(a, b);
                    break;
                default:
                    result = double.MinValue;
                    break;
            }
            return new EToken(result.ToString());
        }

        private double doubleOrFunction(string token)
        {
            if (token.Contains("RAND"))
            {
                return Rand.NextDouble();
            }
            return Convert.ToDouble(token);
        }

        public double SolveAsDouble()
        {
            return Convert.ToDouble(Solve().Value);
        }

        public int SolveAsInt()
        {
            return Convert.ToInt32(SolveAsDouble());
        }
    }
}
