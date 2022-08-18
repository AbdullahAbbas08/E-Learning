using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Risk_Business_Layer.IRepositories.IEmployee;
using Risk_Business_Layer.Repositories.Crud;
using Risk_Business_Layer.Services.AuthenticationModels;
using Risk_Data_Access_Layer;
using Risk_Data_Access_Layer.Constants;
using Risk_Domain_Layer.DTO_S.Employee;

namespace Risk_Business_Layer.Repositories.EmployeeRepo
{
    public class EmployeeRepo : Crud<Employee>, IEmployeeRepo
    {
        private readonly Risk_Data_Access_Layer.DbContext riskDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public EmployeeRepo(Risk_Data_Access_Layer.DbContext riskDbContext , 
                            UserManager<ApplicationUser> userManager 
                            ) : base(riskDbContext)
        {
            this.riskDbContext = riskDbContext; 
            this.userManager = userManager;
        } 

        public GeneralResponseSingleObject<object> DeleteEmployee(string ID)
        {
            try
            {
                GeneralResponseSingleObject<object> response = new GeneralResponseSingleObject<object>();
                var entity = (from x in riskDbContext.employees
                              where x.Id == ID
                              select x).FirstOrDefault();

                if (entity != null)
                {
                    riskDbContext.employees.Remove(entity);
                    riskDbContext.Users.Remove(entity);
                    riskDbContext.SaveChanges();
                    response.Message = "تم حذف الموظف بنجاح";
                }
                else
                    response.Message = "هناك خطأ حاول مرة أخرى";

                return response;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }

        public async Task<GeneralResponse<GetEmployeeDto>> GetAll(string Role)
        {
            string admin = Roles.Admin;
            string agent = Roles.Agent; 

            if (!(Role == admin  || Role == agent))
            {
                return new GeneralResponse<GetEmployeeDto> { Data = null, Message = " Incorrect Role " };
            }

            var usersOfRole = (await  userManager.GetUsersInRoleAsync(Role)).Select(x => x.Id).ToList();

            var employee = from x in riskDbContext.employees
                           where (from y in usersOfRole select y).Contains(x.Id)
                           select x;

            var ListEmployee = ( employee.Select(x =>
           new GetEmployeeDto
           {
               ID = x.Id,
               Address = x.Address,
               Mobile = x.Mobile,
               Name = x.Name,
               NationalId = x.NationalId,
               UserName = x.UserName,
               password = x.PasswordHash
           })).ToList();

            List<GetEmployeeDto> EmpList = new List<GetEmployeeDto>();
            EmpList = ListEmployee.ToList();
            return new GeneralResponse<GetEmployeeDto> { Data = EmpList, Message = "Data returned Successfully" };
        }

       
    }
}
