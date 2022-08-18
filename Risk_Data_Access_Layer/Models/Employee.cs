namespace Risk_Data_Access_Layer.Models
{
    [Table(name: "Employees", Schema = "dbo")]
    public class Employee : ApplicationUser
    {
      
        [Required, Column(TypeName = "varchar(14)"), MaxLength(14),MinLength(14)]
        public string NationalId { get; set; }

    }
    
}
