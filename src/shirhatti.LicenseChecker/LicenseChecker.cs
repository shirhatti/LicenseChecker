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
                    var semanticModel = await document.GetSemanticModelAsync();
                    Console.WriteLine(semanticModel.SyntaxTree.ToString());
                }
            }
            return 0;
        }
    }
}
