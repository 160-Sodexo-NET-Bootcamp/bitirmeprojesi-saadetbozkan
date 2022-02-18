using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Data.UoW;
using ProductCatalog.Entity.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.WepApi.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ProductController> logger;
        private readonly IMapper mapper;
        private IConfiguration configuration;

        public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        //Urün ekleme.
        [HttpPost()]
        public IActionResult PostProduct([FromBody] ProductDetailEntity entity)
        {
            var product = mapper.Map<ProductDetailEntity, Product>(entity);
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            product.OwnerId = Int32.Parse(claim.Value);



            var response = unitOfWork.ProductRepository.Add(product);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }

        //Urün listeleme.
        [HttpGet]
        public IActionResult GetProduct()
        {
            var listOfProducts = unitOfWork.ProductRepository.GetAll();
            if (listOfProducts.Count() == 0)
                return Ok("Ürünler bulunamadı.");

            var productsEntities = mapper.Map<IEnumerable<Product>, IEnumerable<ProductShortEntity>>(listOfProducts);
            return Ok(productsEntities);
        }

        //ürün güncelleme.
        [HttpPut("{id}")]
        public IActionResult PutProduct([FromBody] ProductDetailEntity entity, int id)
        {
            //Ürünün varlığının kontrolü
            var product = unitOfWork.ProductRepository.GetById(id);
            if (product is null)
                return Ok("Ürün bulunamadı!");
            //ürün sahibinin kontrolü yapılır.
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);
            if (userId != product.OwnerId)
                return Ok("Bu işlemi gerçekleştiremezsiniz!");

            product = mapper.Map<ProductDetailEntity, Product>(entity);
            product.Id = id;
            product.OwnerId = userId;
            var response = unitOfWork.ProductRepository.Update(product);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");
            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }

        //ürün silme.
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            //Ürünün varlığının kontrolü
            var product = unitOfWork.ProductRepository.GetById(id);
            if (product is null)
                return Ok("Ürün bulunamadı!");
            // ürün sahibinin kontrolü yapılır. 
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);
            if (userId != product.OwnerId)
                return Ok("Bu işlemi gerçekleştiremezsiniz!");

            var response = unitOfWork.ProductRepository.Delete(id);
            if (response is false)
                return Ok("İşlem gerçekleşemedi.");

            unitOfWork.Complate();
            return Ok("İşlem başarıyla gerçekleşmiştir.");
        }
    }
}
