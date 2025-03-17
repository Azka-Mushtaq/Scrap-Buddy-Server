using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public class EntityService<TEntity> where TEntity : class
    {
        //private readonly IRepository<TEntity> _repository;

        //public EntityService(IRepository<TEntity> repository)
        //{
        //    _repository = repository;
        //}

        //public async Task Add(TEntity entity)
        //{
        //    await _repository.Add(entity);
        //}

        //public async Task Add(string columnNames, TEntity entity, string tableName)
        //{
        //    await _repository.Add(columnNames, entity, tableName);
        //}

        //public async Task<List<TEntity>> GetAll()
        //{
        //    return await _repository.GetAll();
        //}

        //public async Task<List<TEntity>> GetAll(string parameterName, string parameterValue)
        //{
        //    return await _repository.GetAll(parameterName, parameterValue);
        //}

        //public async Task<List<TEntity>> GetAll(string columnNames, string tableName, string parameterName, string parameterValue)
        //{
        //    return await _repository.GetAll(columnNames, tableName, parameterName, parameterValue);
        //}

        //public async Task<TEntity> GetAll(string columnNames, string tableName, string parameterName, string parameterValue, TEntity entity)
        //{
        //    return await _repository.GetAll(columnNames, tableName, parameterName, parameterValue, entity);
        //}

        //public async Task DeleteByPrimaryKey(List<Tuple<string, string>> comparisonColumns, string tableName)
        //{
        //    await _repository.DeleteByPrimaryKey(comparisonColumns, tableName);
        //}

        //public async Task DeleteById(int id, string tableName)
        //{
        //    await _repository.DeleteById(id, tableName);
        //}

        //public async Task DeleteById(string title, string titleValue, string tableName)
        //{
        //    await _repository.DeleteById(title, titleValue, tableName);
        //}

        //public async Task<TEntity> FindByAttribute(string columnNames, string comparisonColumns, string tableName, TEntity entity)
        //{
        //    return await _repository.FindByAttribute(columnNames, comparisonColumns, tableName, entity);
        //}

        ////public async Task<TEntity> FindById(int id)
        ////{
        ////    //return await _repository.Find(id);
        ////}

        //public async Task<TEntity> FindByName(List<Tuple<string, string>> comparisonColumns, TEntity entity)
        //{
       
        //    return await _repository.FindByName(comparisonColumns, entity);
        //}

        //public async Task Update(TEntity entity)
        //{
        //    await _repository.Update(entity);
        //}

        //public async Task Update(string columnNames, TEntity entity, string compName, string compValue, string tableName)
        //{
        //    await _repository.Update(columnNames, entity, compName, compValue, tableName);
        //}
    }
}
