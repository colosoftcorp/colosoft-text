using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Colosoft
{
    internal static class ExpressionExtensions
    {
        private static MemberExpression RemoveUnary(Expression toUnwrap)
        {
            if (toUnwrap is UnaryExpression unaryExpression)
            {
                return (MemberExpression)unaryExpression.Operand;
            }

            return toUnwrap as MemberExpression;
        }

        public static MemberInfo GetMember(this Expression<Func<string>> expression)
        {
            var memberExp = RemoveUnary(expression.Body);

            return memberExp == null ? null : memberExp.Member;
        }
    }
}
