using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Entity.OfferEntities;
using ProductCatalog.Entity.ProductEntities;
using ProductCatalog.Entity.ProductPropertyEntities;
using ProductCatalog.Entity.UserEntities;

namespace ProductCatalog.Core.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserSignupEntity>();
            CreateMap<UserSignupEntity, User>();
            CreateMap<User, UserLoginEntity>();
            CreateMap<UserLoginEntity, User>();

            CreateMap<Color, ColorEntity>();
            CreateMap<ColorEntity, Color>();
            CreateMap<Category, CategoryEntity>();
            CreateMap<CategoryEntity, Category>();
            CreateMap<Trademark, TrademarkEntity>();
            CreateMap<TrademarkEntity, Trademark>();
            CreateMap<UseCase, UseCaseEntity>();
            CreateMap<UseCaseEntity, UseCase>();

            CreateMap<Product, ProductDetailEntity>();
            CreateMap<ProductDetailEntity, Product>();
            CreateMap<Product, ProductShortEntity>();
            CreateMap<ProductShortEntity, Product>();

            CreateMap<Offer, OfferPostEntity>();
            CreateMap<OfferPostEntity, Offer>();
            CreateMap<Offer, OfferGetEntity>();
            CreateMap<OfferGetEntity, Offer>();
        }
    }
}
