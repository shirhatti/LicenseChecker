using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Shirhatti.CodeAnalyzer {
    public static class RuleBuilderExtensions {
        private const string InvokeMethodName = "Invoke";
        public static IRuleBuilder UseRule<TRule>(this IRuleBuilder ruleBuilder, params object[] args)
        {
            return ruleBuilder.UseRule(typeof(TRule), args);
        }

        public static IRuleBuilder UseRule(this IRuleBuilder ruleBuilder, Type Rule, params object[] args)
        {
            return ruleBuilder.Use( next=>
            {
                var methods = Rule.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                var invokeMethods = methods.Where(m => string.Equals(m.Name, InvokeMethodName, StringComparison.Ordinal)).ToArray();
                if (invokeMethods.Length > 1)
                {
                    throw new InvalidOperationException();
                }
                if (invokeMethods.Length == 0)
                {
                    throw new InvalidOperationException();
                }
                var methodinfo = invokeMethods[0];

                if (!typeof(Task).IsAssignableFrom(methodinfo.ReturnType))
                {
                    throw new InvalidOperationException();
                }

                var parameters = methodinfo.GetParameters();
                if (parameters.Length == 0 || parameters[0].ParameterType != typeof(RuleContext))
                {
                    throw new InvalidOperationException();
                }

                var ctorArgs = new object[args.Length + 1];
                ctorArgs[0] = next;
                Array.Copy(args, 0, ctorArgs, 1, args.Length);

                var instance = Activator.CreateInstance(Rule, ctorArgs);

                return (RuleDelegate)methodinfo.CreateDelegate(typeof(RuleDelegate), instance);
            });
        }
    }
}