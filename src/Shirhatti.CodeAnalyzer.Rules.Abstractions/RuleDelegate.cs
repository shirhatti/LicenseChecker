using System.Threading.Tasks;

namespace Shirhatti.CodeAnalyzer.Rules.Abstractions {
    public delegate Task RuleDelegate(RuleContext context);
}