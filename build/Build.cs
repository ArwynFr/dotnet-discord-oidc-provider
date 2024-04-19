using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.OctoVersion;
using Nuke.Common.Utilities.Collections;

class Build : NukeBuild
{
    [Secret, Parameter] readonly string GhToken;
    [Secret, Parameter] readonly string DockerUsername;
    [Secret, Parameter] readonly string DockerPassword;

    [Required, Solution(GenerateProjects = true)] readonly Solution Solution;
    [Required, OctoVersion(AutoDetectBranch = true)] readonly OctoVersionInfo OctoVersionInfo;
    [PathVariable] readonly Tool Gh;
    [PathVariable] readonly Tool Git;

    public static int Main() => Execute<Build>(x => x.Pack);

    bool IsPreRelease => !string.IsNullOrEmpty(OctoVersionInfo.PreReleaseTag);

    Target Pack => _ => _
        .Executes(() => DotNetTasks.DotNetPublish(_ => _
            .SetProject(Solution.ArwynFr_Authentication_Proxy)
            .SetConfiguration(Configuration.Release)
            .AddProperty("ContainerRegistry", string.Empty)
            .AddProperty("ContainerImageTag", OctoVersionInfo.FullSemVer)
            .SetProcessArgumentConfigurator(_ => _.Add("/t:PublishContainer"))));

    Target RunDocker => _ => _
        .DependsOn(Pack)
        .Executes(() => DockerTasks.DockerRun(_ => _
            .SetImage($"arwynfr/discord-oidc-provider:{OctoVersionInfo.FullSemVer}")
            .SetDetach(true)
            .SetRm(true)
            .AddPublish("8080:8080")));

    Target Login => _ => _
        .Unlisted()
        .Requires(() => DockerUsername, () => DockerPassword)
        .Executes(() => DockerTasks.DockerLogin(_ => _
            .SetUsername(DockerUsername)
            .SetPassword(DockerPassword)));

    Target Publish => _ => _
        .DependsOn(Login)
        .Executes(() => DotNetTasks.DotNetPublish(_ => _
            .SetProject(Solution.ArwynFr_Authentication_Proxy)
            .SetConfiguration(Configuration.Release)
            .AddProperty("ContainerImageTag", OctoVersionInfo.FullSemVer)
            .SetProcessArgumentConfigurator(_ => _.Add("/t:PublishContainer"))));


    Target Release => _ => _
        .Unlisted()
        .TriggeredBy(Publish)
        .Requires(() => GhToken)
        .Executes(() => Gh.Invoke(
            arguments: $"release create {OctoVersionInfo.FullSemVer} --generate-notes",
            environmentVariables: EnvironmentInfo.Variables
                .ToDictionary(x => x.Key, x => x.Value)
                .SetKeyValue("GH_TOKEN", GhToken).AsReadOnly()));

    Target Tags => _ => _
        .Unlisted()
        .TriggeredBy(Publish)
        .OnlyWhenStatic(() => !IsPreRelease)
        .Executes(() =>
        {
            Git.Invoke($"tag --force v{OctoVersionInfo.Major}.{OctoVersionInfo.Minor}");
            Git.Invoke($"tag --force v{OctoVersionInfo.Major}");
            Git.Invoke("push origin --tags --force");
        });

}
