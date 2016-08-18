﻿using System;
using Microsoft.Extensions.CommandLineUtils;

namespace shirhatti.LicenseChecker
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "dotnet check-license",
                FullName = "Check License Tool",
                Description = "Static Analyzer to check source code for License Header"
            };
            app.HelpOption("-h|--help");

            var projectPath = app.Argument("<PROJECT>", "The path to the projet (project folder or project.json");

            app.OnExecute(() =>
            {
                var exitCode = new LicenseChecker(projectPath.Value).Run();
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