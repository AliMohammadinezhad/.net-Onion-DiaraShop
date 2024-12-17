using BlogManagement.Application.Contract.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using Framework.Application;

namespace BlogManagement.Application;

public class ArticleApplication : IArticleApplication
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleCategoryRepository _articleCategoryRepository;
    private readonly IFileUploader _fileUploader;

    public ArticleApplication(IArticleRepository articleRepository, IFileUploader fileUploader, IArticleCategoryRepository articleCategoryRepository)
    {
        _articleRepository = articleRepository;
        _fileUploader = fileUploader;
        _articleCategoryRepository = articleCategoryRepository;
    }

    public OperationResult Create(CreateArticle command)
    {
        var operation = new OperationResult();
        if (_articleRepository.Exists(x => x.Title == command.Title))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var publishDate = command.PublishDate.ToGeorgianDateTime();

        var slug = command.Slug.Slugify();
        var categorySlug = _articleCategoryRepository.GetSlugBy(command.CategoryId);

        var path = $"{categorySlug}/{slug}";
        var pictureFile = _fileUploader.Upload(command.Picture, path);

        var article = new Article(command.Title, command.ShortDescription, command.Description, pictureFile, command.PictureAlt,
            command.PictureTitle, slug, command.Keywords, command.MetaDescription, command.CanonicalAddress, command.CategoryId,
            publishDate);

        _articleRepository.Create(article);
        _articleRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditArticle command)
    {
        var operation = new OperationResult();
        var article = _articleRepository.GetWithCategory(command.Id);
        if (article == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        if (_articleRepository.Exists(x => x.Title == command.Title && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = article.Slug.Slugify();
        var path = $"{article.Category.Slug}/{slug}";
        var pictureFile = _fileUploader.Upload(command.Picture, path);
        var publishDate = command.PublishDate.ToGeorgianDateTime();
        article.Edit(command.Title, command.ShortDescription, command.Description, pictureFile, command.PictureAlt,
            command.PictureTitle, slug, command.Keywords, command.MetaDescription, command.CanonicalAddress, command.CategoryId,
            publishDate);
        _articleRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditArticle GetDetails(long id)
    {
        return _articleRepository.GetDetails(id);
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
    {
        return _articleRepository.Search(searchModel);
    }
}