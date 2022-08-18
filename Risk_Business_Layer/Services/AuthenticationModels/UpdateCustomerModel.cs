namespace Risk_Business_Layer.Services.AuthenticationModels
{
    public class UpdateCustomerModel 
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
         
        public string MobileNumber { get; set; }

        public int CityId { get; set; }

        public int Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }
        public string clientId { get; set; }
        public string MobileNumber2 { get; set; }
        public string Phone { get; set; }

    }
}
 