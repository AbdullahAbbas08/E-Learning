﻿namespace Risk_Business_Layer.Services.Authentication
{
   public class AddRoleModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
