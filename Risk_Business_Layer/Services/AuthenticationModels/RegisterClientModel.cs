using Microsoft.AspNetCore.Http;
using Risk_Business_Layer.Services.AuthenticationModels;

namespace Risk_Business_Layer.Services.Authentication
{
    public class RegisterClientModel : RegisterModel
    {  
        public IFormFile? Logo { get; set; }
        public string? LogoPath { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int ClientTypeId { get; set; }
    }
}