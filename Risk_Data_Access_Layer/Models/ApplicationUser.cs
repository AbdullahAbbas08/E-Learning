namespace Risk_Data_Access_Layer.Models
{
    public abstract class ApplicationUser : IdentityUser
    {
        public ApplicationUser() 
        {
            this.CreationDate = DateTime.Now;
        }

        [Required, Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }


        [Required, Column(TypeName = "varchar(11)"), MaxLength(11), Display(Name = "Mobile Number")]
        public string Mobile { get; set; } 


        [Required, Display(Name = "Address"), Column(TypeName = "nvarchar(500)")]
        public string Address { get; set; }

        public DateTime CreationDate { get; set; }


    }

}
