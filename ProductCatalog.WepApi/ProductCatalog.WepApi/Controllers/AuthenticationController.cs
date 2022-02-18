using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductCatalog.Core;
using ProductCatalog.Core.Encodings;
using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Data.UoW;
using ProductCatalog.Entity.UserEntities;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalog.WepApi.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthenticationController> logger;
        private readonly IMapper mapper;
        private IConfiguration configuration;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpPost("/Login")]
        public IActionResult Login([FromBody] UserLoginEntity request)
        {
            //Kayıtı bir Email olup olmdığı tets edildi.
            var users = unitOfWork.UserRepository.Where(x => x.Email.Equals(request.Email));
            if(users.Count == 0)
                return Ok("Bu Email'in mevcut bir kaydı bulunmamaktadır! Lütfen sisteme kaydolunuz!");

            var user = users.FirstOrDefault();

            //Hesabın bloke olma durumunun kontrolü
            if (user.FaultyInputCount >= 3)
                return   Ok("Blocklanmış hesap.");

            //request ile gelen pasword dbdeki ile karşılaştırılması için şifrelenir.
            request.Password = PasswordEncoder.Encoder(request.Password, user.Email);

            //password kontrolü ve hatalı giriş kontrolü
            if (request.Password != user.Password && user.FaultyInputCount < 3)
            {
                user.FaultyInputCount++;
                unitOfWork.UserRepository.Update(user);
                unitOfWork.Complate();

                //Hata satısı 3'e ulaştıysa hesabı blockla ve maile ile bilgilendir.
                if (user.FaultyInputCount == 3)
                {
                    //Hssap bloke olmasına dair e mail atılacak.
                }
                return Ok("Authentication sağlanamadı.");
            }

            // Tokken oluşturuldu.
            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["JWT:Audience"];
            string token = TokenHandler.GenerateToken(key, issuer, audience, user);

            if (string.IsNullOrWhiteSpace(token))
                return Ok("Authentication sağlanamadı.");

            string result = "Authentication sağlandı. Token oluşturuldu. Token {0} : " + token;
            return Ok(result);

        }

        [HttpPost("/Signup")]
        public IActionResult Register([FromBody] UserSignupEntity request)
        {
            //Kayıtı bir Email olup olmdığı tets edildi.
            var isContain = unitOfWork.UserRepository.Where(x => x.Email.Equals(request.Email));
            if (isContain.Count  != 0)
                return Ok("Bu Email'in mevcut bir kaydı bulunmaktadır!");

            // parolanın şifrelenmesi: tuzlanması için bir string değeri ve şifrelenecek parola girildi 
            request.Password = PasswordEncoder.Encoder(request.Password, request.Email);

            //gelen veriyle database modeli maplendi.
            var user = mapper.Map<UserSignupEntity, User>(request);

            //user database eklendi.
            var response = unitOfWork.UserRepository.Add(user);
            unitOfWork.Complate();
            //email gönder.

            return Ok("Kaydınız başarıyla alınmıştır. Lütfen giriş yapınız.");
        }
    }
}
