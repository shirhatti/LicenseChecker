// Copyright (c) Sourabh Shirhatti. All rights reserved.
// Licensed under the MIT License. See LICENSE.md in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using shirhatti.LicenseChecker.Helpers;

namespace shirhatti.LicenseChecker
{
    public class LicenseChecker
    {
        private readonly string _licenseTemplatePath;
        private string _projectPath;
        public LicenseChecker(string licenseTemplatePath, string projectPath) {
            _licenseTemplatePath = licenseTemplatePath;
            _projectPath = projectPath;
        }
        public async Task<int> Run()
        {
            if (string.IsNullOrEmpty(_licenseTemplatePath))
            {
                Console.WriteLine("ERROR: A license template file is required");
                return 1;
            }

            if (!File.Exists(_licenseTemplatePath))
            {
                Console.WriteLine("ERROR: Specified template file does not exist");
                return 1;
            }
            if (string.IsNullOrEmpty(_projectPath))
            {
                _projectPath = Directory.GetCurrentDirectory();
            }

            var template = File.ReadAllText(_licenseTemplatePath).Trim();

            var solution = WorkspaceHelper.Create(_projectPath).FirstOrDefault().CurrentSolution;
            foreach(var project in solution.Projects) {
                foreach (var document in project.Documents) {
                    var semanticModel = await document.GetSemanticModelAsync();
                    var syntaxTree = semanticModel.SyntaxTree;
                    var root = await syntaxTree.GetRootAsync();
                    var leadingTrivia = root.ChildNodes().First().GetLeadingTrivia().ToFullString().Trim();

                    Console.WriteLine((string.Equals(leadingTrivia, template, StringComparison.OrdinalIgnoreCase) ? "SUCCESS" : "FAILURE") + ":\t" + document.Name);
                }
            }
            return 0;
        }
    }
}
