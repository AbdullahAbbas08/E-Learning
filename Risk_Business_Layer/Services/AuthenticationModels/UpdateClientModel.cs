namespace Risk_Business_Layer.Services.Authentication
{
    public class UpdateClientModel: RegisterClientModel
    {
        public string Id { get; set; }
        public string? LogoPath { get; set; }
    }
} 