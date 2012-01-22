using System;
using System.Collections.Generic;
using System.Linq;

namespace Rollout.Utility
{
    public class EToken
    {
        private const string Operators = "+-*/%^";
        private const string LeftAssociativeOperators = "*/%+-";
        private const string RightAssociativeOperators = "^";

        private static readonly Dictionary<string, int> OperatorPrecedence =
            new Dictionary<string, int>
                {
                    {"+-", 2},
                    {"*/%", 3},
                    {"^", 5}
                };

        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (CheckIfNumber(value))
                {
                    IsNumber = true;
                }
                else if (CheckIfOperator(value))
                {
                    IsOperator = true;
                    if (LeftAssociativeOperators.Contains(value))
                        IsLeftAssociative = true;
                    else if (RightAssociativeOperators.Contains(value))
                        IsRightAssociative = true;

                    foreach (var precedenceMapping in OperatorPrecedence.Where(precedenceMapping => precedenceMapping.Key.Contains(value)))
                    {
                        Precedence = precedenceMapping.Value;
                    }
                }
                else if (CheckIfFunction(value))
                {
                    IsFunction = true;
                }
            }
        }

        public bool IsNumber { get; private set; }
        public bool IsFunction { get; private set; }
        public bool IsOperator { get; private set; }
        public bool IsLeftAssociative { get; private set; }
        public bool IsRightAssociative { get; private set; }
        public int Precedence { get; private set; }

        public EToken(string value)
        {
            Value = value;
        }

        private bool CheckIfNumber(string token)
        {
            if (token.Contains('.'))
            {
                double dnoob;
                return double.TryParse(token, out dnoob);
            }
            int inoob;
            return int.TryParse(token, out inoob);
        }

        private bool CheckIfOperator(string token)
        {
            return Operators.Contains(token);
        }

        private bool CheckIfFunction(string token)
        {
            return token.Contains('$');
        }

        public override string ToString()
        {
            return value;
        }
    }

    public static class TokenHelper
    {
        public static List<EToken> GetTokens(string exp)
        {
            var results = new List<EToken>();

            while (exp.Length > 0)
            {
                if (IsNumber(exp[0]))
                {
                    var token = "";
                    while (IsNumber(exp[0]) || exp[0].Equals('.'))
                    {
                        token += exp[0];
                        exp = exp.Substring(1);
                    }
                    results.Add(new EToken(token));
                }
                else if (IsFunction(exp[0]))
                {
                    var token = "";
                    while (!exp[0].Equals('}'))
                    {
                        token += exp[0];
                        exp = exp.Substring(1);
                    }
                    token += exp[0];
                    exp = exp.Substring(1);
                    results.Add(new EToken(token));
                }
                else
                {
                    results.Add(new EToken("" + exp[0]));
                    exp = exp.Substring(1);
                }
            }

            return results;
        }

        private static bool IsNumber(char token)
        {
            int noob;
            return int.TryParse(token.ToString(), out noob);
        }

        private static bool IsFunction(char token)
        {
            return token.Equals('$');
        }
    }

    public static class ReversePolish
    {
        private static readonly Random Rand = new Random();

        public static double SolveAsDouble(List<EToken> tokens)
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
                    stack.Push(DoubleSolve(first, second, token));
                }
            }

            return Convert.ToDouble(stack.Pop().Value);
        }

        private static EToken DoubleSolve(EToken first, EToken second, EToken op)
        {
            var a = RandDouble(first.Value);
            var b = RandDouble(second.Value);
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

        private static double RandDouble(string token)
        {
            return !token.Contains('$') ? Convert.ToDouble(token) : Rand.NextDouble();
        }

        public static int SolveAsInt(List<EToken> tokens)
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
                    stack.Push(IntSolve(first, second, token));
                }
            }

            return Convert.ToInt32(stack.Pop().Value, 10);
        }

        private static EToken IntSolve(EToken first, EToken second, EToken op)
        {
            var a = RandInt(first.Value);
            var b = RandInt(second.Value);
            int result;
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
                    result = (int)Math.Pow(a, b);
                    break;
                default:
                    result = int.MinValue;
                    break;
            }
            return new EToken(result.ToString());
        }

        private static int RandInt(string token)
        {
            if (!token.Contains('$'))
                try
                {
                    return Convert.ToInt32(token, 10);
                }
                catch (Exception e)
                {
                    return Convert.ToInt32(token.Substring(0, token.IndexOf('.')));
                }

            var weights = token.Substring(token.IndexOf('{') + 1);
            weights = weights.Substring(0, weights.IndexOf('}'));
            var minmax = weights.Split(',');
            int min;
            int max;
            var result = int.MinValue;

            if (minmax.Length == 2 && int.TryParse(minmax[0], out min) && int.TryParse(minmax[1], out max))
            {
                result = Rand.Next(min, max);
            }
            else if (minmax.Length == 1 && int.TryParse(minmax[0], out max))
            {

                result = Rand.Next(max);
            }
            else
            {
                result = Rand.Next();
            }

            return result;
        }
    }

    public static class ShuntingYard
    {
        public static List<EToken> Parse(string exp)
        {
            var output = new Queue<EToken>();
            var operators = new Stack<EToken>();
            var tokens = TokenHelper.GetTokens(exp);

            foreach (var token in tokens)
            {
                if (token.IsNumber)
                {
                    output.Enqueue(token);
                }
                else if (token.IsFunction)
                {
                    output.Enqueue(token);
                }
                else if (token.Equals(","))
                {
                    while (operators.Count > 0 && operators.Peek().Value != "(")
                    {
                        var topOperator = operators.Pop();
                        output.Enqueue(topOperator);
                    }
                }
                else if (token.IsOperator)
                {
                    while (operators.Count > 0 && operators.Peek().IsOperator)
                    {
                        if ((token.IsLeftAssociative && operators.Count > 0 && (token.Precedence <= operators.Peek().Precedence) ||
                            (token.IsRightAssociative && (token.Precedence < operators.Peek().Precedence))))
                            output.Enqueue(operators.Pop());
                        else
                            break;
                    }
                    operators.Push(token);
                }

                if (token.Value.Equals("("))
                {
                    operators.Push(token);
                }

                if (token.Value.Equals(")"))
                {
                    while (operators.Count > 0 && operators.Peek().Value != "(")
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Pop();
                }
            }

            while (operators.Count > 0 && operators.Peek().IsOperator)
            {
                output.Enqueue(operators.Pop());
            }

            return output.ToList();
        }
    }
}
