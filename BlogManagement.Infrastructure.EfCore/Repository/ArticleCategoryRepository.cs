using BlogManagement.Application.Contract.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EfCore.Repository;

public class ArticleCategoryRepository : RepositoryBase<long, ArticleCategory>, IArticleCategoryRepository
{
    private readonly BlogContext _blogContext;
    public ArticleCategoryRepository(BlogContext context) : base(context)
    {
        _blogContext = context;
    }

    public string GetSlugBy(long id)
    {
        return _blogContext.ArticleCategories.Select(x => new { x.Id, x.Slug }).FirstOrDefault(x => x.Id == id)?.Slug;
    }

    public List<ArticleCategoryViewModel> GetArticleCategories()
    {
        return _blogContext.ArticleCategories.Select(x => new ArticleCategoryViewModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }

    public EditArticleCategory GetDetails(long id)
    {
        return _blogContext.ArticleCategories.Select(x => new EditArticleCategory
        {
            Id = x.Id,
            Name = x.Name,
            CanonicalAddress = x.CanonicalAddress,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            ShowOrder = x.ShowOrder,
            Slug = x.Slug,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle
        }).FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException();
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
    {
        var query = _blogContext.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryViewModel
        {
            Id = x.Id,
            Picture = x.Picture,
            Description = x.Description,
            Name = x.Name,
            ShowOrder = x.ShowOrder,
            CreationDate = x.CreationDate.ToFarsi(),
            ArticlesCount = x.Articles.Count
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name!.Contains(searchModel.Name));



        return query.OrderByDescending(x => x.ShowOrder).ToList();
    }
}