﻿using Framework.Domain;
using ShopManagement.Contracts.Slide;

namespace ShopManagement.Domain.SlideAgg;

public interface ISlideRepository : IRepository<long, Slide>
{
    EditSlide GetDetails(long id);
    List<SlideViewModel> GetList();
}