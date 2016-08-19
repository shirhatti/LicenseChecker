// Copyright (c) Sourabh Shirhatti. All rights reserved.
// Licensed under the MIT License. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Shirhatti.CodeAnalyzer.Helpers;
using Shirhatti.CodeAnalyzer.Rules;

namespace Shirhatti.CodeAnalyzer
{
    public class Program
    {
        private string _projectPath;

        public Program(string projectPath)
        {
            _projectPath = projectPath;
        }

        public async Task<int> Run()
        {
            if (string.IsNullOrEmpty(_projectPath))
            {
                _projectPath = Directory.GetCurrentDirectory();
            }

            var localDelegate = new RuleBuilder()
                                    .UseRule<LicenseCheckRule>("/Users/shirhatti/src/LicenseChecker/LICENSE.template")
                                    .Build();

            var solution = WorkspaceHelper.Create(_projectPath).FirstOrDefault().CurrentSolution;
            foreach(var project in solution.Projects) {
                foreach (var document in project.Documents) {
                    var semanticModel = await document.GetSemanticModelAsync();
                    var response = new List<string>();
                    var ruleContext = new RuleContext
                    {
                        request = document,
                        response = response
                    };
                    await localDelegate.Invoke(ruleContext);
                    Console.WriteLine(ruleContext.response.ToString());
                }
            }

            return  0;
        }
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "dotnet analyze-code",
                FullName = "Static Code Analyzer",
                Description = "Static Code Analyzer"
            };
            app.HelpOption("-h|--help");

            var projectPath = app.Argument("<PROJECT>", "The path to the projet (project folder or project.json");
            
            app.OnExecute(() =>
            {
                var exitCode = new Program(projectPath.Value).Run();
                return exitCode;
            });

            try{
                return app.Execute(args);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("An error has occured");
                Console.WriteLine(e.ToString());
                return 1;
            }
        }
    }
}
