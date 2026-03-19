using AutoMapper;
using Domain.Entities.Category;
using Infrastructure.Models;

namespace Test
{
    public static class MapperHelper
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
                cfg.CreateMap<CategoryRequestDTO, CategoryModel>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
