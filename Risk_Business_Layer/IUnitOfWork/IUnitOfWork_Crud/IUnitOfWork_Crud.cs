

using Risk_Business_Layer.IRepositories.ICrud;

namespace Risk_Business_Layer.IUnitOfWork.IUnitOfWork_Crud
{
    public interface IUnitOfWork_Crud
    { 
        ICrud<Employee> Employee { get;  }
        Task SaveChangesAsync(); 
    } 
}  
