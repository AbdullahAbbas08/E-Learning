using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Risk_Business_Layer.Helpers;
using Risk_Business_Layer.IRepositories.IEmployee;
using Risk_Business_Layer.Services.Authentication;
using Risk_Business_Layer.Services.AuthenticationModels;
using Risk_Data_Access_Layer.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Risk_Business_Layer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork_Crud unitOfWork;
        private readonly IEmployeeRepo employeeRepo;

        private readonly Helper helper;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            IOptions<JWT> jwt,
                            IUnitOfWork_Crud unitOfWork,
                            IOptions<Helper> _helper,
                            IEmployeeRepo EmployeeRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.unitOfWork = unitOfWork;
            employeeRepo = EmployeeRepo;
            helper = _helper.Value;
            _jwt = jwt.Value;
        }

        public async Task<GeneralResponseSingleObject<UpdateEmployee>> UpdateEmployee(UpdateEmployee model)
        {
            GeneralResponseSingleObject<UpdateEmployee> response = new GeneralResponseSingleObject<UpdateEmployee>();

            var employee = (await unitOfWork.Employee.Find(x => x.Id == model.Id)).FirstOrDefault();

            if (employee != null)
            {
                try
                {
                    employee.Address = model.Address;
                    employee.Mobile = model.Mobile;
                    employee.NationalId = model.NationalId;
                    employee.Name = model.Name;
                    employee.UserName = model.UserName;
                    if (model.password != "***")
                        employee.PasswordHash = _userManager.PasswordHasher.HashPassword(employee, model.password);
                    var result = await _userManager.UpdateAsync(employee);
                    response.Message = "تم تعديل بيانات الموظف بنجاح";
                }
                catch
                {
                    response.Message = "حدث خطأ ما برجاء المحاولة مرة أخرى";
                }

            }
            else
                response.Message = "لا توجد بيانات لهذا الكود";

            return response;
        }
        public async Task<AuthModel> RegisterEmployee(RegisterEmployeeModel model)
        {
            #region model Validation
            string ValidateModel = "";

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                ValidateModel = "User Name is already registered";

            if (string.IsNullOrEmpty(model.Address))
                ValidateModel += " , Address Cannot be Empty";

            if ((model.NationalId).Length < 14)
                ValidateModel += " , National ID Invalid Or Less than 14 Numbers  ";

            if ((model.Mobile).Length < 11)
                ValidateModel += " , Mobile Number Invalid ";

            if (!(Roles.Admin == model.Role || Roles.Agent == model.Role))
                ValidateModel += " , Invalid Role ";




            if (!string.IsNullOrEmpty(ValidateModel))
                return new CreatedUser { Message = ValidateModel };

            #endregion

            #region Fill Employee To Insert
            var user = new Employee
            {
                UserName = model.UserName,
                Address = model.Address,
                Mobile = model.Mobile,
                Name = model.Name,
                NationalId = model.NationalId,
            };
            #endregion

            #region Create Employee 
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }
            #endregion

            #region Assign Default Role To Client
            ApplicationUser ReturenEmployee = await _userManager.FindByIdAsync(user.Id);
            await _userManager.AddToRoleAsync(ReturenEmployee, model.Role);
            #endregion

            #region Create Toaken
            var jwtSecurityToken = await CreateJwtToken(user);
            #endregion



            #region return Authentication Model 
            return new CreatedUser
            {
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                ID_Created = user.Id
            };
            #endregion
        }

        public async Task<AuthModel> Login(TokenRequestModel model)
        {
            #region Generate Object typeOf AuthModel => If UserName & Passowrd : True || Login Successfully
            var authModel = new AuthModel();
            #endregion

            #region Check If User Exist 
            var user = await _userManager.FindByNameAsync(model.UserName);
            #endregion

            #region Check If UserName & Passowrd  =>  True
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Username or Password is incorrect!";
                return authModel;
            }
            #endregion

            #region Create Toaken
            var jwtSecurityToken = await CreateJwtToken(user);
            #endregion
            var roles = await _userManager.GetRolesAsync(user);

            #region Prepare Returned Object
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Role = roles[0].ToString();
            authModel.Id = user.Id;
            authModel.Name = user.Name;
            #endregion

            #region return Object typeOf AuthModel
            return authModel;
            #endregion
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public Task<string> AddRoleAsync(AddRoleModel model)
        {
            throw new NotImplementedException();
        }

        public static string GetPrincipal(string token, IConfiguration _configuration)
        {
            try
            {

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Key"]));

                SecurityToken tokenValidated = null;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:Audience"],

                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:Issuer"],

                };
                ClaimsPrincipal principal = handler.ValidateToken(token, validationParameters, out tokenValidated);
                if (principal == null)
                    return null;
                ClaimsIdentity identity = null;
                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
                var userIdClaim = identity.Claims.ToList();
                string userId = userIdClaim[2].Value;
                return userId;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
