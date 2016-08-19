using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Shirhatti.CodeAnalyzer
{
    public class RuleContext
    {
        public Document request {get; set;}

        public IEnumerable<string> response {get; set;}
    }
}