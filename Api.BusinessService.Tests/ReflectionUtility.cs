using System;
using System.Linq.Expressions;

namespace Api.BusinessService.Tests
{
    internal static class ReflectionUtility
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="prefix">Any prefix string to prepend</param>
        /// <param name="suffix">Any suffix string to append</param>
        /// <returns>prefix + propertyName + suffix</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> expression, string prefix, string suffix)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return prefix + body.Member.Name + suffix;
        }

        /// <summary>
        /// Sets the propertyName as the value of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="expression"></param>
        /// <param name="prefix"></param>
        /// <param name="suffix"></param>
        public static void SetPropertyName<T>(object target, Expression<Func<T>> expression, string prefix, string suffix)
        {
            var propertyName = GetPropertyName(expression, null, null);
            var propertyValue = prefix + propertyName + suffix;
            target.GetType().GetProperty(propertyName).SetValue(target, propertyValue);
        }
    }
}
