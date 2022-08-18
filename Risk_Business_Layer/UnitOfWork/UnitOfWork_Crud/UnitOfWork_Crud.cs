using Risk_Business_Layer.IRepositories.ICrud;
using Risk_Business_Layer.Repositories.Crud;
using Risk_Data_Access_Layer;

namespace Risk_Business_Layer.UnitOfWork.UnitOfWork_Crud
{
    public class UnitOfWork_Crud : IUnitOfWork_Crud
    {
        private readonly DbContext riskDbContext;

        public UnitOfWork_Crud(DbContext riskDbContext){ this.riskDbContext = riskDbContext; }

        private ICrud<Employee> _Employee; 
        public ICrud<Employee> Employee { get { if (_Employee == null) { _Employee = new Crud<Employee>(riskDbContext); } return _Employee; } }

        public async Task SaveChangesAsync()
        {
           await riskDbContext.SaveChangesAsync();   
        }
    }
}
