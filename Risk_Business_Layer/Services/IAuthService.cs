using Risk_Business_Layer.Services.Authentication;
using Risk_Business_Layer.Services.AuthenticationModels;

namespace Risk_Business_Layer.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterEmployee(RegisterEmployeeModel model);
        Task<AuthModel> Login(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<GeneralResponseSingleObject<UpdateEmployee>> UpdateEmployee(UpdateEmployee model);
    }
} 
