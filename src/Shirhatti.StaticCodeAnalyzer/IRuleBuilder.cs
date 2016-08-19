using System;

namespace Shirhatti.CodeAnalyzer {
    public interface IRuleBuilder
    {
        IRuleBuilder Use(Func<RuleDelegate, RuleDelegate> middleware);

        RuleDelegate Build();
        
    }
    
}
