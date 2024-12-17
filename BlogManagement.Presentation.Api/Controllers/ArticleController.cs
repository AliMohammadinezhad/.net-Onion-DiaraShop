using Microsoft.AspNetCore.Mvc;
using Query.Contracts.Article;

namespace BlogManagement.Presentation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticleController : ControllerBase
{
    private readonly IArticleQuery _articleQuery;

    public ArticleController(IArticleQuery articleQuery)
    {
        _articleQuery = articleQuery;
    }

    [HttpGet]
    public List<ArticleQueryModel> GetLatestArrivals()
    {
        return _articleQuery.LatestArticles();
    }
}