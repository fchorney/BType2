using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rollout.Utility.ShuntingYard
{
    /// <summary>
    /// Equation Token
    /// </summary>
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
}
