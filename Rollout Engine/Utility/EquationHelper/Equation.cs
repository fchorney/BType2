using System;
using System.Collections.Generic;

namespace Rollout.Utility.EquationHelper
{
    public class Expression
    {
        private string Value { get; set; }
        private Equation Eq { get; set; }
    

        public Expression(string exp)
        {
            Value = exp;
            Eq = Equation.Parse(Value.Length != 0 ? exp : "0");
        }

        public int AsInt()
        {
            return Eq.SolveAsInt();
        }

        public float AsFloat()
        {
            return 10f;
        }

        public double AsDouble()
        {
            return 10.0;
        }

        public string AsString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Reverse Polish Notation Calculation
    /// </summary>
    public class Equation
    {
        private static readonly Random Rand = new Random();
        private int Sequence;
        private readonly List<EToken> tokens;

        internal Equation(List<EToken> tokens)
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
            if (token.Contains("SEQ"))
            {
                return Sequence;
            }
            return Convert.ToDouble(token);
        }

        public int SolveAsInt()
        {

            Sequence = Convert.ToInt32(Convert.ToDouble(Solve().Value));

            return Sequence;
        }

        public static Equation Parse(string str)
        {
            return ShuntingYard.Parse(str);
        }
    }
}
