using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shirhatti.CodeAnalyzer.Rules {
    public class LicenseCheckRule
    {
        private readonly RuleDelegate _next;
        private readonly string template;

        public LicenseCheckRule(RuleDelegate next, string licenseTemplatePath)
        {
            _next = next;
            if (!File.Exists(licenseTemplatePath))
            {
                throw new FileNotFoundException();
            }
            template = File.ReadAllText(licenseTemplatePath).Trim();
        }

        public async Task Invoke(RuleContext context)
        {
            var semanticModel = await context.request.GetSemanticModelAsync();
            var syntaxTree = semanticModel.SyntaxTree;
            var root = await syntaxTree.GetRootAsync();
            var leadingTrivia = root.ChildNodes().First().GetLeadingTrivia().ToFullString().Trim();
            var licenseCheck = string.Equals(leadingTrivia, template, StringComparison.OrdinalIgnoreCase);
            context.response.Append((licenseCheck ? "SUCCESS" : "FAILURE") + ":\t" + context.request.Name);
            await _next.Invoke(context);
        }
    }
}