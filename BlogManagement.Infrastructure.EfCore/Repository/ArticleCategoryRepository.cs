using BlogManagement.Application.Contract.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EfCore.Repository;

public class ArticleCategoryRepository : RepositoryBase<long, ArticleCategory>,  IArticleCategoryRepository
{
    private readonly BlogContext _blogContext;
    public ArticleCategoryRepository(BlogContext context) : base(context)
    {
        _blogContext = context;
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
            Slug = x.Slug
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
    {
        var query = _blogContext.ArticleCategories.Select(x => new ArticleCategoryViewModel
        {
            Picture = x.Picture,
            Description = x.Description,
            Name = x.Name,
            ShowOrder = x.ShowOrder
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name!.Contains(searchModel.Name));



        return query.OrderByDescending(x => x.ShowOrder).ToList();
    }
}