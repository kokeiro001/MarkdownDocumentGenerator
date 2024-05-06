
namespace MarkdownDocumentGenerator.Renderer
{
    public enum RenderingType
    {
        Default,
        Request,
    }

    public interface IMarkdownRenderer
    {
        ValueTask RenderMarkdown(TypeInfo typeInfo, string repositoryPath, string outputMarkdownFilepath);
    }
}