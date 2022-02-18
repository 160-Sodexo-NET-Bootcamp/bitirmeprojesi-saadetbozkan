using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Data.UoW;
using ProductCatalog.Entity.OfferEntities;
using ProductCatalog.Entity.ProductEntities;
using System;
using System.Linq;

namespace ProductCatalog.WepApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class ProductDetailController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ProductDetailController> logger;
        private readonly IMapper mapper;
        private IConfiguration configuration;

        public ProductDetailController(ILogger<ProductDetailController> logger, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        //ürünü getirme
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //ürünün bulunup bulunmadığı kontrol ediliyor. 
            var product = unitOfWork.ProductRepository.GetById(id);
            if(product is null)
                return Ok("Ürün bulunamadı!");

            var productEntities = mapper.Map<Product, ProductDetailEntity>(product);
            return Ok(productEntities);
        }

        //ürüne teklif verme
        [HttpPost("{id}/offers")]
        public IActionResult PostOffer([FromBody] OfferPostEntity request, int id)
        {
            //ürünün bulunup bulunmadığı kontrol ediliyor.
            var product = unitOfWork.ProductRepository.GetById(id);
            if(product is null)
                return Ok("Ürün bulunamadı!");

            // ürünün teklife açık olup olmadığı kontrol ediliyor.
            if(product.IsOfferable is false)
                return Ok("Teklif verilemez!");
            
            //Ürün bilgilei maplenir.UserId bilgisi jwt tokenden alınır.
            var offer = mapper.Map<OfferPostEntity, Offer>(request);
            offer.ProductId = id;
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            offer.UserId = Int32.Parse(claim.Value);

            //Daha önce bu kullanııcının bu ürüne teklif yapıp yapmadığı kontrol edilir.
            var offersWithProduct = unitOfWork.OfferRepository.Where(x => x.ProductId.Equals(offer.ProductId) ); // generic repository
            var offers = offersWithProduct.Where(x => x.UserId.Equals(offer.UserId)).ToList(); //Linq
            if(offers != null)
                return Ok("Zaten bir teklif yaptınız!");

            //Daha önce teklif yapılmamışsa teklif yapılır.
            var result = unitOfWork.OfferRepository.Add(offer);
            if (result is false)
                return Ok("İşlem gerçekleşmedi");
            unitOfWork.Complate();
            return Ok("Teklif yapıldı.");
        }

        //teklifi silme
        [HttpDelete("{id}/offers/{offerId}")]
        public IActionResult DeleteOffer(int id, int offerId)
        {
            //teklifin bulunup bulunmadığı kontrol ediliyor.
            var offer = unitOfWork.OfferRepository.GetById(offerId);
            if(offer is null)
                return Ok("Teklif bulunamadı!");

            //teklif kullanıcıa aitse silinir.
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);
            if (userId != offer.UserId)
                Ok("Bu işlemi gerçekleştiremezsiniz!");

            var result = unitOfWork.OfferRepository.Delete(offerId);
            if(result is false)
                return Ok("İşlem gerçekleşmedi");
            unitOfWork.Complate();
            return Ok("Teklif silindi.");
        }

        //ürünü alma
        [HttpGet("{id}/buy")]
        public IActionResult BuyProduct(int id)
        {
            //ürünün bulunup bulunmadığı kontrol ediliyor.
            var product = unitOfWork.ProductRepository.GetById(id);
            if(product is null)
                return Ok("Ürün bulunamadı!");

            //Ürüne verilen teklif kabul edilip edilmediği kontrol edilir.
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);

            var listOfOffer = unitOfWork.OfferRepository.Where(x => x.ProductId.Equals(id)); // generic repository
            var offerWithUser = listOfOffer.Where(x => x.UserId.Equals(userId)).FirstOrDefault(); //Linq
            if (offerWithUser.IsApproved is true)
                product.Price = offerWithUser.OfferAmount;

            //Ürünün IsSold değeri güncellendi.
            product.IsSold = true;
            var result = unitOfWork.ProductRepository.Update(product);
            if (result is false)
                return Ok("İşlem gerçekleşmedi");
            unitOfWork.Complate();

            //Bu ürüne ait diğer teklifler reddedilir.
            foreach (var offer in listOfOffer)
            {
                if (offer.UserId == userId)
                    continue;

                offer.IsApproved = false;
                unitOfWork.OfferRepository.Update(offer);
                unitOfWork.Complate();
                //Kullanıcıya email yollandı.
            }
            return Ok("İşlem gerçekleşti!");
        }
    }
}
