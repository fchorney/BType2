using System;
using System.Collections.Generic;
using System.Linq;

namespace Rollout.Utility
{
    public static class ReversePolish
    {
        /// <summary>
        /// Calculates the result of a Reverse Polish Notation Expression
        /// Ex. "4 + 5 * 2" =>" 4 5 + 2 *"
        /// </summary>
        /// <param name="exp">Reverse Polish Notation String</param>
        /// <returns>Solution as Integer</returns>
        public static int Calculate(string exp)
        {
            var stack = new Stack<string>();
            var tokens = exp.Split(' ').ToList();

            foreach (var token in tokens)
            {
                if (ShuntingYard.IsNumber(token))
                {
                    stack.Push(token);
                }
                else if (ShuntingYard.IsOperator(token))
                {
                    var second = stack.Pop();
                    var first = stack.Pop();
                    stack.Push(Operate(first, second, token));
                }
            }

            return Convert.ToInt32(stack.Pop(), 10);
        }

        private static string Operate(string first, string second, string op)
        {
            var a = Convert.ToInt32(first, 10);
            var b = Convert.ToInt32(second, 10);
            switch (op)
            {
                case "+":
                    return (a + b).ToString();
                case "-":
                    return (a - b).ToString();
                case "*":
                    return (a * b).ToString();
                case "/":
                    return (a / b).ToString();
                case "%":
                    return (a % b).ToString();
                case "^":
                    return Math.Pow(a, b).ToString();
                default:
                    return int.MinValue.ToString();
            }
        }
    }

    public static class ShuntingYard
    {
        private const string Operators = "+-*/%^";
        private const string LeftAssociativeOperators = "*/%+-";
        private const string RightAssociativeOperators = "^";

        private static readonly Dictionary<string, int> operatorPrecedence =
            new Dictionary<string, int>
                {
                    {"+-", 2},
                    {"*/%", 3},
                    {"^", 5}
                };

        private static readonly Random Rand = new Random();

        /// <summary>
        /// Converts Infix Notation into Reverse Polish Notation.
        /// Ex. "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3" => "3 4 2 * 1 5 − 2 3 ^ ^ / +"
        /// Supports the $RAND keyword.
        /// Ex. $RAND or $RAND{max} or $RAND{min,max}
        /// </summary>
        /// <param name="exp">Infix Notation String</param>
        /// <returns>Reverse Polish Notation String</returns>
        public static string Parse(string exp)
        {
            var output = new Queue<string>();
            var operators = new Stack<string>();
            var tokens = exp.Split(' ').ToList();

            foreach (var token in tokens)
            {
                if (IsNumber(token))
                {
                    output.Enqueue(token);
                }
                else if (IsRand(token))
                {
                    output.Enqueue(GetRand(token).ToString());
                }
                else if (token.Equals(","))
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        var topOperator = operators.Pop();
                        output.Enqueue(topOperator);
                    }
                }
                else if (IsOperator(token))
                {
                    while (operators.Count > 0 && IsOperator(operators.Peek()))
                    {
                        if ((IsLeftAssociative(token) && operators.Count > 0 && (GetPrecedenceFor(token) <= GetPrecedenceFor(operators.Peek()))) || (IsRightAssociative(token) && (GetPrecedenceFor(token) < GetPrecedenceFor(operators.Peek()))))
                        {
                            output.Enqueue(operators.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }
                    operators.Push(token);
                }

                if (token.Equals("("))
                {
                    operators.Push(token);
                }

                if (token.Equals(")"))
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Pop();
                }
            }

            while (operators.Count > 0 && IsOperator(operators.Peek()))
            {
                output.Enqueue(operators.Pop());
            }

            var result = string.Empty;
            while (output.Count() > 0)
            {
                result += output.Dequeue() + " ";
            }

            return result.Trim();
        }

        public static bool IsNumber(string token)
        {
            int noob;
            return int.TryParse(token, out noob);
        }

        public static bool IsOperator(string token)
        {
            return Operators.Contains(token);
        }

        private static bool IsRand(string token)
        {
            return token.ToUpper().Contains("$RAND");
        }

        private static int GetRand(string token)
        {
            if (token.IndexOf('{') == -1)
            {
                return Rand.Next();
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
            else if (int.TryParse(minmax[0], out max))
            {

                result = Rand.Next(max);
            }

            return result;
        }

        private static bool IsLeftAssociative(string token)
        {
            return LeftAssociativeOperators.Contains(token);
        }

        private static bool IsRightAssociative(string token)
        {
            return RightAssociativeOperators.Contains(token);
        }

        private static int GetPrecedenceFor(string token)
        {
            foreach (var precedenceMapping in operatorPrecedence)
            {
                if (precedenceMapping.Key.Contains(token))
                    return precedenceMapping.Value;
            }
            return int.MinValue;
        }
    }
}
