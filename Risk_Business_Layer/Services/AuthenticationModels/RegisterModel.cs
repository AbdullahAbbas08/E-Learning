namespace Risk_Business_Layer.Services.AuthenticationModels
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName  { get; set; }
        
        [Required]
        public string Password  { get; set; }

        [Required, MaxLength(11)]
        public string Mobile { get; set; }

        [Required]
        public string Address { get; set; }
        public string Role { get; set; }
    }
}