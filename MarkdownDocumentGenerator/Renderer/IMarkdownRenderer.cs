
namespace MarkdownDocumentGenerator.Renderer
{
    public interface IMarkdownRenderer
    {
        ValueTask RenderMarkdown(TypeInfo typeInfo, string repositoryPath, string outputMarkdownFilepath);
    }
}