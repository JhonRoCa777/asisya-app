using AutoMapper;
using Domain.Entities;
using Domain.Entities.Category;
using Domain.Entities.Product;
using Domain.Entities.Supplier;
using Infrastructure.Models;

namespace Infrastructure.Config
{
    public class Profile_Mapper : Profile
    {
        public Profile_Mapper()
        {
            // CATEGORY
            CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
            CreateMap<CategoryRequestDTO, CategoryModel>().ReverseMap();

            // SUPPLIER
            CreateMap<SupplierDTO, SupplierModel>().ReverseMap();

            // EMPLOYEE
            CreateMap<EmployeeDTO, EmployeeModel>().ReverseMap();
            CreateMap<EmployeeDTO, EmployeeModel>().ReverseMap();

            // SUPPLIER
            CreateMap<SupplierModel, SupplierToRef>();

            // CATEGORY
            CreateMap<CategoryModel, CategoryToRef>();

            // PRODUCT
            CreateMap<ProductModel, ProductDTO>();
            CreateMap<ProductModel, ProductToIndex>()
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        }
    }
}
