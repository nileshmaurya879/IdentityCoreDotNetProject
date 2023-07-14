using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Data;
using ProductAPI.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        public readonly ProductContext _productContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ProductRepository(ProductContext productContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
            _productContext = productContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public Product GetProduct()
        {
            var test = _productContext.Products.ToList();
            return test[0];
        }


        #region Login Account

        public async Task<IdentityResult> RegistrationAsyns(SignUpModel signUpModel)
        {
            // check user exist or not
            var existUser = await _userManager.FindByEmailAsync(signUpModel.Email);
            if (existUser != null)
                return null;


            //// Register new user
            var user = new ApplicationUser()
            {
                Email = signUpModel.Email,
                UserName = signUpModel.UserName,
            };
            var signupUser = await _userManager.CreateAsync(user, signUpModel.Password);

            //Create Roles if does not exists
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            var userRoleName = _roleManager.Roles.Where(x=>x.Id == signUpModel.RoleId.ToString()).Select(x=>x.Name).FirstOrDefault();
           
            if(userRoleName != null)
                await _userManager.AddToRoleAsync(user, userRoleName.ToString());

            return null;

        }

        public async Task<string> LoginAsync(SignInModel signUpModel)
        {
            var user = await _userManager.FindByNameAsync(signUpModel.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, signUpModel.Password,false);

            if (!result.Succeeded)
            {
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

          
            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
               // new Claim(ClaimTypes.Email,user.Email),
                //new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                //new Claim("Id",user.Id.ToString()),
               // new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authsignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var tocken = new JwtSecurityToken(
                     issuer: _configuration["JWT:ValidIssuer"],
                     audience: _configuration["JWT:ValidAudience"],
                     expires: DateTime.Now.AddDays(1),
                     claims: authClaim,
                     signingCredentials: new SigningCredentials(authsignInKey,SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(tocken);
        }
        #endregion Login Account

        #region User Role
        public async Task<IdentityResult> CreateUserRole(UserRole user)
        {

            var userRoleTest = UserRoles.User;

            var userRole = await _roleManager.CreateAsync(user);

            return userRole;
        }
        #endregion User Role
    }
}
