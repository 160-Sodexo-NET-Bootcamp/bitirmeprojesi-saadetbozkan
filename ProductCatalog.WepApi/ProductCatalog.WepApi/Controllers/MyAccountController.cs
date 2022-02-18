using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductCatalog.Core.Encodings;
using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Data.UoW;
using ProductCatalog.Entity.OfferEntities;
using ProductCatalog.Entity.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.WepApi.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class MyAccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<MyAccountController> logger;
        private readonly IMapper mapper;
        private IConfiguration configuration;

        public MyAccountController(ILogger<MyAccountController> logger, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        //Kullanıcının yaptığı teklifler.
        [HttpGet("giveOffers")]
        public IActionResult GiveAllOffers()
        {
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);

            //kullanıcının yaptığı teklifler.
            var offerList = unitOfWork.OfferRepository.Where(x => x.UserId.Equals(userId));
            if (offerList.Count == 0)
                return Ok("Henüz bir teklif yapılmadı!");

            //teklif yapılan ürün satıldıysa:
            var products = offerList.Select(x => x.ProductId ).ToList();
            List<Offer> offers = null;
            foreach (var productId in products)
            {
                var product = unitOfWork.ProductRepository.GetById(productId);
                if (product.IsSold is true)
                    continue;
                offers.Add(offerList.FirstOrDefault(x => x.ProductId.Equals(product.Id)));
            }
            if(offers.Count == 0)
                return Ok("Teklif verdiğiniz ürün kalmadı.");

            var offerEntities = mapper.Map<IList<Offer>, IList<OfferGetEntity>>(offers);
            return Ok(offerEntities);
        }

        //Kullanıcının aldığı teklifler
        [HttpGet("getOffers")]
        public IActionResult GetAllOffers()
        {
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);

            //kullanıcıya ait ürünler listelenir. Bu ürünlere gelen teklifler alınır.
            var productList = unitOfWork.ProductRepository.Where(x => x.OwnerId.Equals(userId));

            if (productList.Count() == 0)
                return Ok("Ürününüz bulunmuyor!");

            IList<Offer> offerList = null;
            foreach(var product in productList)
            {
                var offers = unitOfWork.OfferRepository.Where(x => x.ProductId.Equals(product.Id)).ToList();
                if (offers.Count != 0)
                   offerList.ToList().AddRange(offers);
            }
            var offerEntities = mapper.Map<IList<Offer>, IList<OfferGetEntity>>(offerList);
            if (offerEntities.Count() == 0)
                return Ok("Henüz bir teklif alınmadı!");

            return Ok(offerEntities);
        }

        //Kullanıcının aldığı tekliflere cevapları.
        [HttpPost("GetOffers/{offerId}")]
        public IActionResult Approve([FromBody] AnswerForOfferEntity answer, int offerId)
        {
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);

            //teklifin bulunup bulunmadığı kontrol ediliyor. 
            var offer = unitOfWork.OfferRepository.GetById(offerId);
            if (offer is null)
                return Ok("Teklif bulunamadı!");

            //ürünün sahibi olup oladığına bakılır.
            var product = unitOfWork.ProductRepository.GetById(offer.ProductId);
            if(userId != product.OwnerId)
                return Ok("Bu işlemi gerçekleştiremezsinzi!");

            if (answer.IsApproved is "Onayla")
            {
                offer.IsApproved = true;
                unitOfWork.OfferRepository.Update(offer);
                unitOfWork.Complate();
                return Ok("Teklif onaylandı.");
                //email gönder
            }

            if (answer.IsApproved is "Reddet")
            {
                offer.IsApproved = false;
                unitOfWork.OfferRepository.Update(offer);
                unitOfWork.Complate();
                return Ok("Teklif onaylanmadı.");

                //email gönder
            }
            return Ok();
        }

        //Parolayı değiştirme
        [HttpPut] //("password")
        public IActionResult ChangePassword([FromBody] UserChangePasswordEntity request)
        {
            var claim = HttpContext.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault();
            var userId = Int32.Parse(claim.Value);

            var user = unitOfWork.UserRepository.GetById(userId);

            //Parolanın doğru olup olmadığı kontrol edilir.
            var password = PasswordEncoder.Encoder(request.Password, user.Email);
            if (password != user.Password)
                return Ok("Hatalı parola!");

            //Yeni parola şifrelenip kaydedildi.
            var newPassword = PasswordEncoder.Encoder(request.NewPassword, user.Email);
            user.Password = newPassword;
            var resonse = unitOfWork.UserRepository.Update(user);
            if (resonse is false)
                return Ok("İşlem gerçekleşmedi!");

            unitOfWork.Complate();
            return Ok("İşlem gerçekleşti!");
        }
    }
}
