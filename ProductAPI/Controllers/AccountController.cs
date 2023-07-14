using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Model;
using ProductAPI.Repository;
using System;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public AccountController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(SignUpModel signUpModel)
        {
            
            var signUpUser = await _productRepository.RegistrationAsyns(signUpModel);

            if (signUpUser != null && signUpUser.Succeeded)
            {
                return Ok( new { Message = "User Created Successfully...!" }  );
            }
            return Ok(new { Message = "User Not Created...!" });
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginAsync(SignInModel signInModel)
        {
            var signInUser = await _productRepository.LoginAsync(signInModel);

            if (string.IsNullOrEmpty(signInUser))
            {
                return Unauthorized();
            }
            return Ok(signInUser);
        }


        [HttpPost("CreateUserRole")]
        public async Task<IActionResult> CreateUserRole(UserRole userRole)
        {
            var user = await _productRepository.CreateUserRole(userRole);
            if(user.Succeeded)
            {
                return Ok(user.Succeeded);
            }
            return Ok(null);
        }

    }
}
