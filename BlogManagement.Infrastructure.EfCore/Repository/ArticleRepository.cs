using System.Xml.Linq;
using BlogManagement.Application.Contract.Article;
using BlogManagement.Domain.ArticleAgg;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EfCore.Repository;

public class ArticleRepository : RepositoryBase<long, Article>, IArticleRepository
{
    private readonly BlogContext blogContext;
    public ArticleRepository(BlogContext context) : base(context)
    {
        blogContext = context;
    }

    public EditArticle GetDetails(long id)
    {
        return blogContext.Articles.Select(x => new EditArticle
        {
            CanonicalAddress = x.CanonicalAddress,
            CategoryId = x.CategoryId,
            Description = x.Description,
            Slug = x.Slug,
            Id = x.Id,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ShortDescription = x.ShortDescription,
            PublishDate = x.PublishDate.ToFarsi(),
            Title = x.Title
        }).FirstOrDefault(x => x.Id == id);
    }

    public Article GetWithCategory(long id)
    {
        return blogContext.Articles.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
    {
        var query = blogContext.Articles.Include(x => x.Category).Select(x => new ArticleViewModel
        {
            Id = x.Id,
            Category = x.Category.Name,
            Picture = x.Picture,
            Title = x.Title,
            PublishDate = x.PublishDate.ToFarsi(),
            ShortDescription = x.ShortDescription,
            CategoryId = x.CategoryId
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Title))
            query = query.Where(x => x.Title.Contains(searchModel.Title));

        if (searchModel.CategoryId > 0)
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
}