using Microsoft.AspNetCore.Identity;
using ProductAPI.Data;
using ProductAPI.Model;
using System.Threading.Tasks;

namespace ProductAPI.Repository
{
    public interface IProductRepository
    {
        Product GetProduct();

        #region login Account

        public Task<IdentityResult> RegistrationAsyns(SignUpModel signUpModel);

        Task<string> LoginAsync(SignInModel signUpModel);

        #endregion login Account

        #region User Role

        Task<IdentityResult> CreateUserRole(UserRole user);

        #endregion User Role
    }
}
