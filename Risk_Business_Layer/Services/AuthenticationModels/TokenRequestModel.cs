using System.ComponentModel.DataAnnotations;
namespace Risk_Business_Layer.Services.Authentication
{
    public class TokenRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
