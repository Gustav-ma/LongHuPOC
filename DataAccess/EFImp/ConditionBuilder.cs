﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace LongHu.DataAccess.EFImp
{
    public class ConditionBuilder : ExpressionVisitor
    {
        private List<object> m_arguments;
        private Stack<string> m_conditionParts;

        public string Condition { get; private set; }

        public object[] Arguments { get; private set; }

        public void Build(Expression expression)
        {
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(expression);

            this.m_arguments = new List<object>();
            this.m_conditionParts = new Stack<string>();

            this.Visit(evaluatedExpression);

            this.Arguments = this.m_arguments.ToArray();
            this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : null;
        }

        public override Expression VisitBinary(BinaryExpression b)
        {
            if (b == null) return b;

            string opr = string.Empty;
            switch (b.NodeType)
            {
                case ExpressionType.Equal:
                    opr = "=";
                    break;
                case ExpressionType.NotEqual:
                    opr = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    opr = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    opr = ">=";
                    break;
                case ExpressionType.LessThan:
                    opr = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    opr = "<=";
                    break;
                case ExpressionType.AndAlso:
                    if (b.Left.NodeType == ExpressionType.Constant) return this.VisitBinary((BinaryExpression)b.Right);
                    opr = "AND";

                    break;
                case ExpressionType.OrElse:
                    if (b.Left.NodeType == ExpressionType.Constant) return this.VisitBinary((BinaryExpression)b.Right);
                    opr = "OR";
                    break;
                case ExpressionType.Add:
                    opr = "+";
                    break;
                case ExpressionType.Subtract:
                    opr = "-";
                    break;
                case ExpressionType.Multiply:
                    opr = "*";
                    break;
                case ExpressionType.Divide:
                    opr = "/";
                    break;
                case ExpressionType.Or:
                    opr = "or";
                    if (b.Left.NodeType != ExpressionType.Constant)
                    {
                        this.VisitBinary((BinaryExpression)b.Left);
                        this.VisitBinary((BinaryExpression)b.Right);
                    }
                    else
                    {
                        return this.VisitBinary((BinaryExpression)b.Right);

                    }

                    break;
                default:
                    throw new NotSupportedException(b.NodeType + " is not supported.");
            }

            this.Visit(b.Left);
            this.Visit(b.Right);

            string right = this.m_conditionParts.Pop();
            string left = this.m_conditionParts.Pop();

            string condition = String.Format("({0} {1} {2})", left, opr, right);
            this.m_conditionParts.Push(condition);

            return b;
        }

        public override Expression VisitConstant(ConstantExpression c)
        {
            if (c == null) return c;

            this.m_arguments.Add(c.Value);
            this.m_conditionParts.Push(String.Format("{{{0}}}", this.m_arguments.Count - 1));

            return c;
        }

        public override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m == null) return m;

            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            if (propertyInfo == null) return m;

            this.m_conditionParts.Push(String.Format("[{0}]", propertyInfo.Name));

            return m;
        }

        public override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) return m;

            string format;
            switch (m.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;

                default:
                    throw new NotSupportedException(m.NodeType + " is not supported!");
            }

            this.Visit(m.Object);
            this.Visit(m.Arguments[0]);
            string right = this.m_conditionParts.Pop();
            string left = this.m_conditionParts.Pop();
            this.m_conditionParts.Push(String.Format(format, left, right));

            return m;
        }
    }
}


