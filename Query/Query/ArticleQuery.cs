using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EfCore;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Query.Contracts.Article;

namespace Query.Query;

public class ArticleQuery : IArticleQuery
{
    private readonly BlogContext _context;

    public ArticleQuery(BlogContext context)
    {
        _context = context;
    }

    public ArticleQueryModel GetArticleDetails(string slug)
    {
        var article = _context.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Slug = x.Slug,
                Picture = x.Picture,
                CategorySlug = x.Category.Slug,
                CanonicalAddress = x.CanonicalAddress,
                CategoryName = x.Category.Name,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
                Title = x.Title

            }).FirstOrDefault(x => x.Slug == slug);
        if (article != null)
        {
            article.KewordList = article.Keywords.Split("،").ToList();
        }

        return article;
    }

    public List<ArticleQueryModel> LatestArticles()
    {
        return _context.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
                Title = x.Title
            }).ToList();
    }
}