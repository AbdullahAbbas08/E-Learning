using Risk_Business_Layer.Services.AuthenticationModels;
using Risk_Domain_Layer.DTO_S.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_Business_Layer.IRepositories.IEmployee
{
    public interface IEmployeeRepo
    {
        Task<GeneralResponse<GetEmployeeDto>> GetAll(string Role);
        GeneralResponseSingleObject<object> DeleteEmployee(string ID);  
    }
}
 