using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eventualize.Dapper.Materialization
{
    public class EventKeyProperty
    {
        public PropertyInfo Property { get; set; }

        public string ParameterName { get; set; }
    }

    public class KeyComparer
    {
        public string KeyCompareClause { get; set; }

        public IList<EventKeyProperty> EventKeyProperties { get; set; }

        public KeyComparer()
        {
            this.EventKeyProperties = new List<EventKeyProperty>();
        }
    }

    public class KeyCompareExpressionVisitor
    {
        private Type projectionModelType;

        private Type eventModelType;

        private KeyComparer keyComparer;

        public KeyCompareExpressionVisitor(Type projectionModelType, Type eventModelType)
        {
            this.projectionModelType = projectionModelType;
            this.eventModelType = eventModelType;
        }

        public KeyComparer ComputeKeyComparision(LambdaExpression keyCompareExpression)
        {
            this.keyComparer = new KeyComparer();

            this.keyComparer.KeyCompareClause = this.VisitLambda(keyCompareExpression);

            return this.keyComparer;
        }

        private string VisitLambda(LambdaExpression keyCompareExpression)
        {
            return this.Visit(keyCompareExpression.Body);
        }

        private string Visit(Expression expression)
        {
            BinaryExpression expressionAsBinary = (BinaryExpression)expression;
            if (!IsComparison(expressionAsBinary))
            {
                return this.VisitCombination(expressionAsBinary);
            }
            else
            {
                return this.VisitComparison(expressionAsBinary);
            }
        }

        private string VisitCombination(BinaryExpression combination)
        {
            var left = this.Visit(combination.Left);
            var sqlOperator = this.GetSqlOperator(combination.NodeType);
            var right = this.Visit(combination.Right);

            return $"{left} {sqlOperator} {right}";
        }

        private string VisitComparison(BinaryExpression comparison)
        {
            var left = this.VisitProperty((MemberExpression)comparison.Left);
            var right = this.VisitProperty((MemberExpression)comparison.Right);
            var sqlOperator = this.GetSqlOperator(comparison.NodeType);


            return $"{left} {sqlOperator} {right}";
        }

        private string VisitProperty(MemberExpression memberExpression)
        {
            string parameterName = "";
            var commandParameterIndicator = "";
            if (memberExpression.Member.ReflectedType == this.eventModelType)
            {
                parameterName = $"@Event_{memberExpression.Member.Name}";
                this.keyComparer.EventKeyProperties.Add(new EventKeyProperty()
                {
                    ParameterName = parameterName,
                    Property = (PropertyInfo)memberExpression.Member
                });
        }
            else
            {
                parameterName = memberExpression.Member.Name;
            }

            return parameterName;
        }

private bool IsComparison(BinaryExpression binaryExpression)
{
    switch (binaryExpression.NodeType)
    {
        case ExpressionType.Equal:
        case ExpressionType.NotEqual:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
            return true;
        default:
            return false;
    }
}

private string GetSqlOperator(ExpressionType expressionType)
{
    switch (expressionType)
    {
        case ExpressionType.AndAlso:
            return "and";
        case ExpressionType.Equal:
            return "=";
        case ExpressionType.OrElse:
            return "or";
        case ExpressionType.NotEqual:
            return "<>";
        case ExpressionType.GreaterThan:
            return ">";
        case ExpressionType.LessThan:
            return "<";
        case ExpressionType.GreaterThanOrEqual:
            return ">=";
        case ExpressionType.LessThanOrEqual:
            return "<=";
        default:
            throw new NotImplementedException($"Operator for {expressionType} not supported");
    }
}
    }
}
