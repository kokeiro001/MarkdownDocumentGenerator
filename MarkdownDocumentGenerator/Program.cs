using LibGit2Sharp;
using MarkdownDocumentGenerator.Renderer;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Configuration;

namespace MarkdownDocumentGenerator
{
    public class Program
    {
        static async Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build()
                .Get<Config>();

            Config.Validate(config);

            if (!Directory.Exists(config.OutputMarkdownDirectory))
            {
                Directory.CreateDirectory(config.OutputMarkdownDirectory);
            }

            MSBuildLocator.RegisterDefaults();
            using var workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(config.ProjectPath);

            var typeInfoCollector = new TypeInfoCollector(project);

            var typeInfos = await typeInfoCollector.Collect(config.TargetBaseTypeName);

            var repositoryPath = Repository.Discover(Path.GetDirectoryName(config.ProjectPath));

            var renderer = new DefaultMarkdownRenderer();

            foreach (var typeInfo in typeInfos)
            {
                var outputMarkdownFilepath = Path.Combine(config.OutputMarkdownDirectory, $"{typeInfo.DisplayName}.md");
                await renderer.RenderMarkdown(typeInfo, repositoryPath, outputMarkdownFilepath);
            }
        }
    }
}
