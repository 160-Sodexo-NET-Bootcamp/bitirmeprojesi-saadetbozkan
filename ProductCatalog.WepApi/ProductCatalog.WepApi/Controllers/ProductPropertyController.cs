using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Data.UoW;
using ProductCatalog.Entity.ProductPropertyEntities;
using System.Collections.Generic;
using ProductCatalog.Entity.ProductEntities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ProductCatalog.WepApi.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class ProductPropertyController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ProductPropertyController> logger;
        private readonly IMapper mapper;
        private IConfiguration configuration;

        public ProductPropertyController(ILogger<ProductPropertyController> logger, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        //Ürünlerin bulunduğu kategorilerle ilgili işlemler :
        //Bütün kategorileri getirir.
        [HttpGet("/categories")]
        public IActionResult GetCategories()
        {
            var listOfCategories = unitOfWork.CategoryRepository.GetAll();
            if (listOfCategories.Count() == 0)
                return Ok("Kategoriler bulunamadı.");

            var categoryEntities = mapper.Map<IEnumerable<Category>, IEnumerable<CategoryEntity>>(listOfCategories);
            return Ok(categoryEntities);
        }
        //İstenilen kategoriye ait ürünleri getirir.
        [HttpGet("/categories/{id}")]
        public IActionResult GetByCategoryId(int id)
        {
            //Kategorinin bulunup bulunmadığı kontrol ediliyor. 
            var category = unitOfWork.CategoryRepository.GetById(id);
            if (category is null)
                return Ok("Kategori bulunamadı!");
            //Kategoriye ait ürünler yoksa bütün ürünler listelenir.
            var products = unitOfWork.ProductRepository.Where(x => x.CategoryId.Equals(id));
            if (products.Count == 0)
                products = unitOfWork.ProductRepository.GetAll().ToList();
            //Kategoriye ait ürünler listelenir.
            var productEntities = mapper.Map<IEnumerable<Product>, IEnumerable<ProductShortEntity>>(products);
            return Ok(productEntities);
        }
        //Kategori ekleme.
        [HttpPost("/categories")]
        public IActionResult PostCategory([FromBody] CategoryEntity entity)
        {
            var category = unitOfWork.CategoryRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault();
            if(category != null)
                return Ok("Bu kategori zaten var!");

            category = mapper.Map<CategoryEntity, Category>(entity);

            var response = unitOfWork.CategoryRepository.Add(category);
            if(response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //kategori güncelleme.
        [HttpPut("/categories/{id}")]
        public IActionResult PutCategory([FromBody] CategoryEntity entity, int id)
        {
            var category = unitOfWork.CategoryRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault();
            if (category != null)
                return Ok("Bu kategori zaten var!");

            //kategorinin olup olmadığına bakılır.
             category = unitOfWork.CategoryRepository.GetById(id);
            if (category is null)
                return Ok("Kategori bulunmuyor!");

            category = mapper.Map<CategoryEntity, Category>(entity);
            category.Id = id;

            var response = unitOfWork.CategoryRepository.Update(category);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //kategori silme.
        [HttpDelete("/categories/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            //kategorinin olup olmadığına bakılır.
            var category = unitOfWork.CategoryRepository.GetById(id);
            if (category is null)
                return Ok("Kategori bulunmuyor!");

            var response = unitOfWork.CategoryRepository.Delete(id);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }

        //Ürünlerin renkleriyle ilgili işlemler :
        //Bütün renkleri getirir.
        [HttpGet("/colors")]
        public IActionResult GetColors()
        {
            var listOfColors = unitOfWork.ColorRepository.GetAll();
            if (listOfColors.Count() == 0)
                return Ok("Renkler bulunamadı.");

            var colorEntities = mapper.Map<IEnumerable<Color>, IEnumerable<ColorEntity>>(listOfColors);
            return Ok(colorEntities);
        }
        //İstenilen renge ait ürünleri getirir.
        [HttpGet("/colors/{id}")]
        public IActionResult GetByColorId(int id)
        {
            //Rengin bulunup bulunmadığı kontrol ediliyor. 
            var color = unitOfWork.ColorRepository.GetById(id);
            if (color is null)
                return Ok("Renk bulunamadı!");

            //Renge ait ürünler yoksa bütün ürünler listelenir.
            var products = unitOfWork.ProductRepository.Where(x => x.ColorId.Equals(id));
            if (products is null)
                products = unitOfWork.ProductRepository.GetAll().ToList();

            //Renge ait ürünler listelenir.
            var productEntities = mapper.Map<IEnumerable<Product>, IEnumerable<ProductShortEntity>>(products);
            return Ok(productEntities);
        }
        //Renk ekleme.
        [HttpPost("/colors")]
        public IActionResult PostColor([FromBody] ColorEntity entity)
        {
            var color = unitOfWork.ColorRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault(); ;
            if (color != null)
                return Ok("Bu renk zaten mevcut!");

            color = mapper.Map<ColorEntity, Color>(entity);
            var response = unitOfWork.ColorRepository.Add(color);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //renk güncelleme.
        [HttpPut("/colors/{id}")]
        public IActionResult PutColor([FromBody] ColorEntity entity, int id)
        {
            var color = unitOfWork.ColorRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault(); ;
            if (color != null)
                return Ok("Bu renk zaten mevcut!");

            color = unitOfWork.ColorRepository.GetById(id) ;
            if (color is null)
                return Ok("Renk bulunamadı!");

            

            color = mapper.Map<ColorEntity, Color>(entity);
            color.Id = id;

            var response = unitOfWork.ColorRepository.Update(color);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //renk silme.
        [HttpDelete("/colors/{id}")]
        public IActionResult DeleteColor(int id)
        {
            var color = unitOfWork.ColorRepository.GetById(id);
            if (color is null)
                return Ok("Renk bulunamadı!");

            var response = unitOfWork.ColorRepository.Delete(id);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }

        //Ürünlerin markalarıyla ilgili işlemler :
        //Bütün markaları getirir.
        [HttpGet("/trademarks")]
        public IActionResult GetTrademarks()
        {
            var listOfTrademarks = unitOfWork.TrademarkRepository.GetAll();
            if (listOfTrademarks.Count() == 0)
                return Ok("Marka bulunamadı.");

            var trademarkEntities = mapper.Map<IEnumerable<Trademark>, IEnumerable<TrademarkEntity>>(listOfTrademarks);
            return Ok(trademarkEntities);
        }
        //İstenilen markaya ait ürünleri getirir.
        [HttpGet("/trademarks/{id}")]
        public IActionResult GetByTrademarkId(int id)
        {
            //Markanın bulunup bulunmadığı kontrol ediliyor. 
            var trademark = unitOfWork.TrademarkRepository.GetById(id);
            if (trademark is null)
                return Ok("Marka bulunamadı!");
            //Markaya ait ürünler yoksa bütün ürünler listelenir.
            var products = unitOfWork.ProductRepository.Where(x => x.Trademark.Equals(id));
            if (products.Count() == 0)
                products = unitOfWork.ProductRepository.GetAll().ToList();

            //Markaya ait ürünler listelenir.
            var productEntities = mapper.Map<IEnumerable<Product>, IEnumerable<ProductShortEntity>>(products);
            return Ok(productEntities);
        }
        //Marka ekleme.
        [HttpPost("/trademarks")]
        public IActionResult PostTrademark([FromBody] TrademarkEntity entity)
        {
            //Markanın bulunup bulunmadığı kontrol ediliyor. 
            var trademark = unitOfWork.TrademarkRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault(); ;
            if (trademark != null)
                return Ok("Bu marka zaten mevcut!");

            trademark = mapper.Map<TrademarkEntity, Trademark>(entity);

            var response = unitOfWork.TrademarkRepository.Add(trademark);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //marka güncelleme.
        [HttpPut("/trademarks/{id}")]
        public IActionResult PutTrademark([FromBody] TrademarkEntity entity, int id)
        {
            //Markanın bulunup bulunmadığı kontrol ediliyor. 
            var trademark = unitOfWork.TrademarkRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault(); ;
            if (trademark != null)
                return Ok("Bu marka zaten mevcut!");

            trademark = unitOfWork.TrademarkRepository.GetById(id);
            if (trademark is null)
                return Ok("Marka bulunamadı!");

            trademark = mapper.Map<TrademarkEntity, Trademark>(entity);
            trademark.Id = id;

            var response = unitOfWork.TrademarkRepository.Update(trademark);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //marka silme.
        [HttpDelete("/trademarks/{id}")]
        public IActionResult DeleteTrademark(int id)
        {
            var trademark = unitOfWork.TrademarkRepository.GetById(id);
            if (trademark is null)
                return Ok("Marka bulunamadı!");

            var response = unitOfWork.TrademarkRepository.Delete(id);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }

        //Ürünlerin kullanım durumuyla ilgili işlemler :
        //Bütün kullanım durumlarını getirir.
        [HttpGet("/usecases")]
        public IActionResult GetUseCases()
        {
            var listOfUseCases = unitOfWork.UseCaseRepository.GetAll();
            if (listOfUseCases.Count() == 0)
                return Ok("Kullanım durumları bulunamadı.");

            var useCaseEntities = mapper.Map<IEnumerable<UseCase>, IEnumerable<UseCaseEntity>>(listOfUseCases);
            return Ok(useCaseEntities);
        }
        //İstenilen kullanım durumuna ait ürünleri getirir.
        [HttpGet("/usecases/{id}")]
        public IActionResult GetByUseCaseId(int id)
        {
            //Kullanım durumunun bulunup bulunmadığı kontrol ediliyor. 
            var useCase = unitOfWork.UserRepository.GetById(id);
            if (useCase is null)
                return Ok("Kullanım durumu bulunamadı!");

            //Kullanım durumuna ait ürünler yoksa bütün ürünler listelenir.
            var products = unitOfWork.ProductRepository.Where(x => x.UseCaseId.Equals(id));
            if (products.Count() == 0)
                products  = unitOfWork.ProductRepository.GetAll().ToList();

            //Kullanım durumuna ait ürünler listelenir.
            var productEntities = mapper.Map<IEnumerable<Product>, IEnumerable<ProductShortEntity>>(products);
            return Ok(productEntities);
        }
        //useCase ekleme.
        [HttpPost("/usecases")]
        public IActionResult PostUseCase([FromBody] UseCaseEntity entity)
        {
            var useCase = unitOfWork.UseCaseRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault(); ;
            if (useCase != null)
                return Ok("Bu kullanım durumu zaten mevcut!");

            useCase = mapper.Map<UseCaseEntity, UseCase>(entity);

            var response = unitOfWork.UseCaseRepository.Add(useCase);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //useCase güncelleme.
        [HttpPut("/usecases/{id}")]
        public IActionResult PutUseCase([FromBody] UseCaseEntity entity, int id)
        {
            var useCase = unitOfWork.UseCaseRepository.Where(x => x.Name.Equals(entity.Name)).FirstOrDefault(); ;
            if (useCase != null)
                return Ok("Bu kullanım durumu zaten mevcut!");

             useCase = unitOfWork.UseCaseRepository.GetById(id) ;
            if (useCase is null)
                return Ok("Kullanım durumu bulunamadı!");

            useCase = mapper.Map<UseCaseEntity, UseCase>(entity);
            useCase.Id = id;

            var response = unitOfWork.UseCaseRepository.Update(useCase);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
        //useCase silme.
        [HttpDelete("/usecases/{id}")]
        public IActionResult DeleteUseCase(int id)
        {
            var useCase = unitOfWork.UseCaseRepository.GetById(id);
            if (useCase is null)
                return Ok("Kullanım durumu bulunamadı!");

            var response = unitOfWork.UseCaseRepository.Delete(id);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
    }
}
