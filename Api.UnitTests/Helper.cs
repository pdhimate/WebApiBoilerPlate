using Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http.Filters;

namespace Api.UnitTests
{
    /// <summary>
    /// Project-wide helper methods for unit tests
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Determines whether the specified method has the attribute.
        /// </summary>
        /// <typeparam name="TParentClass"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static bool MethodHasAttribute<TParentClass, TAttribute>(string methodName) where TAttribute : class
        {
            var method = typeof(TParentClass).GetMethods()
                                             .SingleOrDefault(x => x.Name == methodName);

            var attribute = method.GetCustomAttributes(typeof(TAttribute), true).Single() as TAttribute;
            return attribute != null;
        }

        internal static bool ControllerHasAttribute<TController, TAttribute>(TController controller)
              where TController : BaseApiController
        {
            return controller.GetType().IsDefined(typeof(TAttribute), false);
        }
        internal static TAttribute GetAttribute<TController, TAttribute>(TController controller)
            where TController : BaseApiController where TAttribute : FilterAttribute
        {
            return controller.GetType().GetCustomAttribute<TAttribute>();
        }
        internal static List<MethodInfo> GetPublicActions<TReturnType>(BaseApiController baseApiController)
        {
            var allMethodInfos = baseApiController.GetType().GetMethods();
            var publicActions = allMethodInfos.Where(mi => mi.ReturnType == typeof(TReturnType)
                                         && mi.DeclaringType == typeof(BaseApiController)
                                         && mi.IsPublic);
            return publicActions.ToList();
        }

        internal static bool ActionHasAttribute<TController, TActionResult, TAttribute>(
                Expression<Func<TController, TActionResult>> actionExpression)
        {
            var methodCall = (MethodCallExpression)actionExpression.Body;
            var methodInfo = methodCall.Method;
            return methodInfo.GetCustomAttributes(false)
                             .Any(a => a is TAttribute);
        }
        internal static TAttribute GetAttribute<TController, TActionResult, TAttribute>(
                Expression<Func<TController, TActionResult>> actionExpression)
            where TAttribute : class
        {
            var methodCall = (MethodCallExpression)actionExpression.Body;
            var methodInfo = methodCall.Method;
            return GetAttribute<TAttribute>(methodInfo);
        }
        internal static TAttribute GetAttribute<TAttribute>(MethodInfo methodInfo)
            where TAttribute : class
        {
            return methodInfo.GetCustomAttributes(false)
                             .FirstOrDefault(a => a is TAttribute) as TAttribute;
        }
    }
}
