using Cake.Common.IO;
using Cake.Common.Solution;
using Cake.Core;
using Cake.Core.Diagnostics;
using SimpleGitVersion;
using System.Linq;

namespace CodeCake
{
    [AddPath("packages/**/tools*")]
    [AddPath("%UserProfile%/.nuget/packages/**/tools*")]
    public partial class Build : CodeCakeHost
    {
        public Build()
        {
            Cake.Log.Verbosity = Verbosity.Diagnostic;

            var configuration = "Debug";
            var solutionName = "ITI-Human.sln";
            var projects = Cake.ParseSolution(solutionName)
                           .Projects
                           .Where(p => !(p is SolutionFolder)
                                       && p.Name != "CodeCakeBuilder");

            Task("Clean")
                .Does(() =>
                {
                    Cake.CleanDirectories("**/bin/" + configuration, d => !d.Path.Segments.Contains("CodeCakeBuilder"));
                    Cake.CleanDirectories("**/obj/" + configuration, d => !d.Path.Segments.Contains("CodeCakeBuilder"));
                });

            Task("Build")
                .IsDependentOn("Clean")
                .Does(() =>
                {
                    StandardSolutionBuild(solutionName, Cake.GetSimpleRepositoryInfo(), configuration);
                });

            Task("Test")
                .IsDependentOn("Build")
                .Does(() =>
                {
                    StandardUnitTests(configuration, projects.Where(p => p.Name.EndsWith(".Tests")));
                });

            Task("Default")
                .IsDependentOn("Test")
                .Does(() =>
                {
                    // ...
                });
        }
    }
}
