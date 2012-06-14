using System.Collections.Generic;
using System.Linq;

namespace Rollout.Utility.EquationHelper
{
    /// <summary>
    /// Shunting Yard turns Infix Notation in to Postfix Notation
    /// </summary>
    public class ShuntingYard
    {
        internal static Equation Parse(string exp)
        {
            var output = new Queue<EToken>();
            var operators = new Stack<EToken>();
            var tokens = GetTokens(exp);

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

            return new Equation(output.ToList());
        }

        internal static List<EToken> GetTokens(string exp)
        {
            var results = new List<EToken>();

            while (exp.Length > 0)
            {
                if (IsNumber(exp[0]))
                {
                    var token = "";
                    while (exp != "" && (IsNumber(exp[0]) || exp[0].Equals('.')))
                    {
                        token += exp[0];
                        exp = exp.Substring(1);
                    }
                    results.Add(new EToken(token));
                }
                else if (IsFunction(exp[0]))
                {
                    var token = "" + exp.Substring(0, 1);
                    exp = exp.Substring(1);
                    while (!IsFunction(exp[0]))
                    {
                        token += exp[0];
                        exp = exp.Substring(1);
                    }
                    token += exp[0];
                    exp = exp.Substring(1);
                    results.Add(new EToken(token.Substring(0, token.Length - 1)));
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
}
