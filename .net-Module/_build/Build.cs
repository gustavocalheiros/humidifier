namespace Build;

using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    readonly Solution Solution;

    Project TestProject;

    Target Init => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            TestProject = Solution.GetProject("WeatherStats_Tests");
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(x => x.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(x => x
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration));
        });

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .DependsOn(Init)
        .Executes(() =>
        {
            DotNetTest(x => x
                .SetProjectFile(TestProject)
                .SetConfiguration(Configuration)
                .SetLoggers("trx;LogFileName=../../../TestResults.xml"));
        });
}
