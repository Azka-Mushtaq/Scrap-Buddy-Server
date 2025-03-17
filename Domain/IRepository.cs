using System.Collections.Generic;

namespace Domain
{
    public interface IRepository<TEntity>
    {

        public Task<int> Add(TEntity entity);
        public Task<int> Add(string columnNames, TEntity entity, string tableName, bool requireId = true);
        public Task<List<TEntity>> GetAll(string columnNames = "", string tableName = "", string compParameter = "", string compValue = "");

        //public Task Delete(List<Tuple<string, string>> comparisonColumns);

         public Task DeleteById(int id);
        // public Task DeleteById(string title, string titleValue, string tableName);
        public  Task<TEntity> FindByAttribute(string columnNames, string comparisonColumns, string comparisonValues);
        public Task<TEntity> Find(string columnNames = "", string compParameter = "Id", string compValue = "");
        public Task Update(TEntity entity);

        public Task Update(string columnNames, TEntity entity);

    }
}
