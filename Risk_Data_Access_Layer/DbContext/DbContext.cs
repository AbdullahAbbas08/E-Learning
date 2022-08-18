namespace Risk_Data_Access_Layer
{
    public class DbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbContext(DbContextOptions<DbContext> options):base(options)
        {
           
        }
        public DbSet<Employee> employees { get; set; }         
         
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Change Table Name Exist In Identity
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            //builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            //Remove Column Exist In Identity
            builder.Entity<IdentityUser>()
                .Ignore(e => e.Email)
                .Ignore(e => e.EmailConfirmed)
                .Ignore(e => e.UserName); 
            
           

            //Change Table Name && Remove Column Exist In Identity
            //builder.Entity<IdentityUser>()
            //    .ToTable("Users")
            //    .Ignore(e => e.Email);

            //builder.Entity<ClientCustomerServise>()
            //.HasKey(t => new { t.ClientId, t.CustomerId }); 
         
        }
         
    }
}
