using System;

namespace Shirhatti.CodeAnalyzer.Rules.Abstractions {
    public interface IRuleBuilder
    {
        IRuleBuilder Use(Func<RuleDelegate, RuleDelegate> middleware);

        RuleDelegate Build();
        
    }
    
}
