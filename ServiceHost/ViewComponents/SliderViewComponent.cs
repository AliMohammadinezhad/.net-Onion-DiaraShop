using Microsoft.AspNetCore.Mvc;
using Query.Contracts.Slide;

namespace ServiceHost.ViewComponents;

public class SliderViewComponent : ViewComponent
{
    private readonly ISlideQuery _slideQuery;

    public SliderViewComponent(ISlideQuery slideQuery)
    {
        _slideQuery = slideQuery;
    }

    public IViewComponentResult Invoke()
    {
        var slides = _slideQuery.GetSlides();
        return View(slides);
    }
}