using AutoMapper;
using PetFeast_Backend2.Models.AddressModels;
using PetFeast_Backend2.Models.AddressModels.DTOs;
using PetFeast_Backend2.Models.CategoryModels;
using PetFeast_Backend2.Models.CategoryModels.DTOs;
using PetFeast_Backend2.Models.ProductModels;
using PetFeast_Backend2.Models.ProductModels.DTOs;
using PetFeast_Backend2.Models.UserModels;
using PetFeast_Backend2.Models.UserModels.DTOs;
using PetFeast_Backend2.Models.WishListModels;
using PetFeast_Backend2.Models.WishListModels.DTOs;

namespace PetFeast_Backend2.Mappings
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            // User to UserRegisterDTO and vice versa mapping
            CreateMap<User, UserRegisterDTO>().ReverseMap();

            // Category to CategoryCreateDTO and vice versa mapping
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();

            // Category to CategoryResDTO and vice versa mapping
            CreateMap<Category, CategoryResDTO>().ReverseMap();

            // Product to ProductCreateDTO and vice versa mapping
            CreateMap<Product, ProductCreateDTO>().ReverseMap();

            // WishList to WishListCreateDTO and vice versa mapping
            CreateMap<WishList, WishListCreateDTO>().ReverseMap();

            // User to UserResDTO and vice versa mapping
            CreateMap<User, UserResDTO>().ReverseMap();

            // Address to AddressResDTO and vice versa mapping
            CreateMap<Address, AddressResDTO>().ReverseMap();
        }
    }
}
