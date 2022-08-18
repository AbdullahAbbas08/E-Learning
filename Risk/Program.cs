using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Risk_Business_Layer.IRepositories.ICrud;
using Risk_Business_Layer.Business_Logic.Business;
using Risk_Business_Layer.IUnitOfWork.IUnitOfWork_Crud;
using Risk_Business_Layer.Repositories.Crud;
using Risk_Business_Layer.Seeds;
using Risk_Business_Layer.Services;
using Risk_Business_Layer.UnitOfWork.UnitOfWork_Crud;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Risk_Business_Layer.IRepositories.IEmployee;
using Risk_Business_Layer.Repositories.EmployeeRepo;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Mapped Interfaces Implementations 
builder.Services.AddTransient(typeof(ICrud<>), typeof(Crud<>));
builder.Services.AddTransient<IUnitOfWork_Crud, UnitOfWork_Crud>();
builder.Services.AddTransient<IEmployeeRepo, EmployeeRepo>();

builder.Services.AddTransient<IAdminBusiness, AdminBusiness>();
builder.Services.AddScoped<IAuthService, AuthService>(); 
#endregion

#region Map Classes Into Appsettings
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<Helper>(builder.Configuration.GetSection("PATHS"));
#endregion

#region Apply Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Risk_Data_Access_Layer.DbContext>();
#endregion

#region Add CORS
builder.Services.AddCors();
#endregion

#region Add Context
builder.Services.AddDbContext<Risk_Data_Access_Layer.DbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),options =>
    {
        options.CommandTimeout(180); // 3 minutes
     } ));
#endregion

#region Swagger Configurations
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //Swagger Header
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Risk API",
        Description = "Description Not Added Yet "
    });

    //Swagger Definition For All 
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    //Swagger Definition For One Controller
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
                },
                Name = "Bearer",
                    In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
#endregion

#region Authentecation Service Configurations (JWT)
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],
    ValidAudience = builder.Configuration["JWT:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
});
#endregion

var app = builder.Build();

#region Seed Users And Roles
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerProvider>();
var logger = loggerFactory.CreateLogger("app");

try
{
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DefaultRoles.SeedRole(roleManager);
    await DefaultUsers.SeedAgentUser(userManager);
    await DefaultUsers.SeedAdminUser(userManager, roleManager);

    logger.LogInformation("Data seeded");
    logger.LogInformation("Application Started");
}
catch (System.Exception ex)
{
    logger.LogWarning(ex, "An error occurred while seeding data");
}

#endregion

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
#endregion

#region Allow CORS 
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
#endregion

#region To Use Identity
app.UseAuthentication();
app.UseAuthorization();
#endregion

app.MapControllers();


app.Run();
