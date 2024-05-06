using LibGit2Sharp;
using RazorEngineCore;

namespace MarkdownDocumentGenerator.Renderer
{
    public class DefaultMarkdownRenderer
    {
        private static readonly TimeZoneInfo jstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

        public class RenderMarkdownModel(TypeInfo typeInfo)
        {
            public TypeInfo TypeInfo { get; } = typeInfo;

            public DateTimeOffset RenderDateTime { get; init; }

            public string RenderProjectGitBranch { get; init; } = "";

            public string RenderProjectGitCommitHash { get; init; } = "";
        }

        public async Task RenderMarkdown(
            TypeInfo typeInfo,
            string repositoryPath,
            string outputMarkdownFilepath)
        {
            var markdownTemplate = await File.ReadAllTextAsync("DefaultMarkdownTemplate.cshtml");
            var razorEngine = new RazorEngine();
            var template = await razorEngine.CompileAsync(markdownTemplate);

            var gitRepositoryInfo = GetRepositoryInfo(repositoryPath);

            var renderMarkdownModel = new RenderMarkdownModel(typeInfo)
            {
                RenderDateTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, jstTimeZone),
                RenderProjectGitBranch = gitRepositoryInfo.currentBranchName,
                RenderProjectGitCommitHash = gitRepositoryInfo.lastCommitHash,
            };

            var result = await template.RunAsync(renderMarkdownModel);
            await File.WriteAllTextAsync(outputMarkdownFilepath, result);

            Console.WriteLine($"Finish RenderMarkdown {renderMarkdownModel.TypeInfo.DisplayName}");
        }

        private static (string currentBranchName, string lastCommitHash) GetRepositoryInfo(string repositoryPath)
        {
            using var repo = new Repository(repositoryPath);

            // 現在のブランチ名を取得
            var currentBranchName = repo.Head.FriendlyName;

            // 最新のコミットのハッシュを取得
            var latestCommitHash = repo.Head.Tip.Sha;

            return (currentBranchName, latestCommitHash);
        }
    }
}
