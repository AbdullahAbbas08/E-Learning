using Microsoft.EntityFrameworkCore;
using Risk_Business_Layer.IRepositories.ICrud;
using Risk_Data_Access_Layer;
using System.Linq.Expressions;


namespace Risk_Business_Layer.Repositories.Crud
{
    public class Crud<T> : ICrud<T> where T : class
    {
        #region Declare Variables
        private readonly Risk_Data_Access_Layer.DbContext riskDbContext;
        #endregion

        #region Constructor
        public Crud(Risk_Data_Access_Layer.DbContext riskDbContext)
        {
            this.riskDbContext = riskDbContext;
        }
        #endregion

        #region Add
        public virtual async Task<T> Add(T Entity)
        {
           await riskDbContext.AddAsync(Entity);
           return Entity;
        }

        public async Task<List<T>> AddRange(List<T> Entities)
        {
            await riskDbContext.AddRangeAsync(Entities);
            return Entities;
        }
        #endregion

        #region Delete
        public virtual async Task<bool> Delete(int ID)
        {
            T Entity = await Find(ID);


            if (Entity != null)
            {
                riskDbContext.Remove(Entity);
            }
            else
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Find
        public virtual async Task<T> Find(int ID)
        {
            return await riskDbContext.FindAsync<T>(ID);
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await riskDbContext.Set<T>()
                .AsQueryable()
                .Where(predicate).ToListAsync();
        }
        #endregion

        #region Get With Condition
        public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> match)
        {
           return await riskDbContext.Set<T>().AsQueryable().Where(match).ToListAsync();
        }
        #endregion

        #region GetAll
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return  await riskDbContext.Set<T>().AsQueryable().ToListAsync();
        }
        #endregion

        #region Update
        public virtual T Update(T Entity)
        {
            var UpdatedEntity = riskDbContext.Update(Entity).Entity;
            return UpdatedEntity;
        }
        #endregion

    }
}
