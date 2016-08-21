using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shirhatti.CodeAnalyzer {
    public class RuleBuilder : IRuleBuilder
    {
        private readonly IList<Func<RuleDelegate, RuleDelegate>> _rules = new List<Func<RuleDelegate, RuleDelegate>>();

        public RuleDelegate Build()
        {
            RuleDelegate app = ruleContext =>
            {
                return Task.FromResult(0);
            };
            foreach (var rule in _rules)
            {
                app = rule(app);
            }
            return app;
        }

        public IRuleBuilder Use(Func<RuleDelegate, RuleDelegate> rule)
        {
            _rules.Add(rule);
            return this;
        }

    }
}