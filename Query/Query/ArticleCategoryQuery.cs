using System.Runtime.InteropServices.ComTypes;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EfCore;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Query.Contracts.Article;
using Query.Contracts.ArticleCategory;

namespace Query.Query;

public class ArticleCategoryQuery : IArticleCategoryQuery
{
    private readonly BlogContext _context;

    public ArticleCategoryQuery(BlogContext context)
    {
        _context = context;
    }

    public ArticleCategoryQueryModel getArticleCategoryBySlug(string slug)
    {
        var articleCategory = _context.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryQueryModel
        {
            Slug = x.Slug,
            Name = x.Name,
            Description = x.Description,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            CanonicalAddress = x.CanonicalAddress,
            Articles = MapArticles(x.Articles),
            ArticleCount = x.Articles.Count,
            Id = x.Id,
            ShowOrder = x.ShowOrder,

        }).FirstOrDefault(x => x.Slug == slug);

        if(!string.IsNullOrWhiteSpace(articleCategory?.Keywords))
            articleCategory.KeywordList = articleCategory.Keywords.Split("،").ToList();
        
        return articleCategory;
    }

    private static List<ArticleQueryModel> MapArticles(List<Article> articles)
    {
        return articles.Select(x => new ArticleQueryModel
        {
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Picture = x.Picture,
            Title = x.Title,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            PublishDate = x.PublishDate.ToFarsi()
        }).ToList();
    }

    public List<ArticleCategoryQueryModel> GetArticleCategories()
    {
        return _context.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryQueryModel
            {
                Id = x.Id,
                Slug = x.Slug,
                Picture = x.Picture,
                CanonicalAddress = x.CanonicalAddress,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Name = x.Name,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ShowOrder = x.ShowOrder,
                ArticleCount = x.Articles.Count,
            }).ToList();
    }
}