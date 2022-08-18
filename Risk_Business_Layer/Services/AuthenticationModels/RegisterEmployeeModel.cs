using Risk_Business_Layer.Services.AuthenticationModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Risk_Business_Layer.Services.Authentication
{
    public class RegisterEmployeeModel: RegisterModel
    {
        [Required, Column(TypeName = "varchar(14)"), MaxLength(14), MinLength(14), Display(Name = "National Id")]
        public string NationalId { get; set; }
    }
     
}
