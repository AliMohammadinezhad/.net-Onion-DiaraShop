namespace BlogManagement.Application.Contract.ArticleCategory;

public class ArticleCategoryViewModel
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Picture { get; set; }
    public required string Description { get; set; }
    public int ShowOrder { get; set; }
    public required string CreationDate { get; set; }
    public long ArticlesCount { get; set; }
}