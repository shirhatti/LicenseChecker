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
        private string _projectPath;
        public LicenseChecker(string projectPath) {
            _projectPath = projectPath;
        }
        public async Task<int> Run()
        {
            if (string.IsNullOrEmpty(_projectPath))
            {
                _projectPath = Directory.GetCurrentDirectory();
            }

            var solution = WorkspaceHelper.Create(_projectPath).FirstOrDefault().CurrentSolution;
            foreach(var project in solution.Projects) {
                foreach (var document in project.Documents) {
                    Console.WriteLine(document.Name);
                    var semanticModel = await document.GetSemanticModelAsync();
                    var syntaxTree = semanticModel.SyntaxTree;
                    var root = await syntaxTree.GetRootAsync();
                    var leadingTrivia = root.ChildNodes().First().GetLeadingTrivia();
                    Console.WriteLine(leadingTrivia.ToFullString());
                    Console.WriteLine("\n");
                }
            }
            return 0;
        }
    }
}
