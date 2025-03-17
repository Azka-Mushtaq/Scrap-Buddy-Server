using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;

using Domain;
using System.Diagnostics;
using Dapper;

namespace Infrastructure
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
    {
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScrapBuddy;Integrated Security=True;";

        public GenericRepository() { }

        public async Task<int> Add(TEntity entity)
        {
            try
            {
                var tableName = typeof(TEntity).Name;
                var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id" && p.Name != "File");

                var columnName = string.Join(",", properties.Select(x => x.Name));
                var parameterNames = string.Join(",", properties.Select(y => "@" + y.Name));

                string query = $"INSERT INTO [{tableName}] ({columnName}) OUTPUT INSERTED.Id VALUES({parameterNames})";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var parameters = new DynamicParameters();

                    foreach (var property in properties)
                    {
                        parameters.Add("@" + property.Name, property.GetValue(entity));
                    }

                    var id = await conn.ExecuteScalarAsync<int>(query, parameters);
                    return Convert.ToInt32(id);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception type: {ex.GetType}, Message: {ex.Message}");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> Add(string columnNames, TEntity entity, string tableName, bool requireId = true)
        {
            try
            {
                var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id");

                var list = columnNames.Split(',');
                var parameterNames = string.Empty;
                for (int i = 0; i < list.Length; i++)
                {
                    if (i == list.Length - 1)
                        parameterNames += ("@" + list[i]);
                    else
                        parameterNames += ("@" + list[i] + ",");
                }

                string query;

                if (requireId == true)
                    query = $"insert into [{tableName}] ({columnNames}) OUTPUT INSERTED.Id values({parameterNames})";
                else
                    query = $"insert into [{tableName}] ({columnNames}) values({parameterNames})";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var parameters = new DynamicParameters();

                    foreach (var property in properties)
                    {
                        if (list.Contains(property.Name))
                        {
                            parameters.Add("@" + property.Name, property.GetValue(entity));
                        }
                    }
                    var id = await conn.ExecuteScalarAsync<int>(query, parameters);
                    return Convert.ToInt32(id);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }


        public async Task<List<TEntity>> GetAll(string columnNames = "", string tableName = "",
            string compParameter = "", string compValue = "")
        {
            try
            {
                if (tableName == "")
                    tableName = typeof(TEntity).Name;

                string query = "";
                if (columnNames == "")
                    query = $"SELECT * FROM [{tableName}]";
                else
                    query = $"SELECT {columnNames} FROM [{tableName}]";

                if (compParameter != "")

                    query += $" where {compParameter} = @compValue";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    if (compParameter == "")
                    {
                        var result = await connection.QueryAsync<TEntity>(query);
                        return result.AsList();
                    }
                    else
                    {
                        var result = await connection.QueryAsync<TEntity>(query, new { compValue = compValue });
                        return result.AsList();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Trace: Exception type: {ex.GetType}, Message: {ex.Message}");
                Debug.WriteLine($"Debug: Exception type: {ex.GetType}, Message: {ex.Message}");
                throw new Exception("No Data Found", ex);
            }
        }

        //public async Task Delete(List<Tuple<string, string>> comparisonColumns)
        //{
        //    try
        //    {
        //        string tableName = typeof(TEntity).Name;
        //        string whereCondition = string.Empty;


        //        for (int i = 0; i < comparisonColumns.Count; i++)
        //        {
        //            if (i == comparisonColumns.Count - 1)
        //                whereCondition += (comparisonColumns[i].Item1 + " = @" + comparisonColumns[i].Item1);
        //            else
        //                whereCondition += (comparisonColumns[i].Item1 + " = @" + comparisonColumns[i].Item1 + " and ");

        //        }
        //        string query = $"DELETE FROM [{tableName}] where {whereCondition}";

        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            foreach (var tup in comparisonColumns)
        //                cmd.Parameters.AddWithValue("@" + tup.Item1, tup.Item2);

        //            int rowsAffected = await cmd.ExecuteNonQueryAsync();

        //            // Throw an exception if no rows were deleted
        //            if (rowsAffected == 0)
        //            {
        //                throw new Exception("Error: No Data Found");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine($"Trace: Exception type: {ex.GetType}, Message: {ex.Message}");
        //        Debug.WriteLine($"Debug: Exception type: {ex.GetType}, Message: {ex.Message}");
        //        throw new Exception("No Data Found", ex);
        //    }

        //}

        public async Task DeleteById(int id)
        {
            try
            {

                var tableName = typeof(TEntity).Name;

                string query = $"DELETE FROM [{tableName}] where Id= @Id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Id", id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Trace: Exception type: {ex.GetType}, Message: {ex.Message}");
                Debug.WriteLine($"Debug: Exception type: {ex.GetType}, Message: {ex.Message}");
                throw new Exception(ex.Message, ex);
            }

        }
        //public async Task DeleteById(string title, string titleValue, string tableName)
        //{

        //    try
        //    {
        //        string whereCondition = string.Empty;


        //        string query = $"DELETE FROM {tableName} where {title}= @titleValue";

        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            cmd.Parameters.AddWithValue("@titleValue", titleValue);

        //            await cmd.ExecuteNonQueryAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine($"Trace: Exception type: {ex.GetType}, Message: {ex.Message}");
        //        Debug.WriteLine($"Debug: Exception type: {ex.GetType}, Message: {ex.Message}");
        //        throw new Exception("No Data Found", ex);
        //    }

        //}
        public async Task<TEntity> FindByAttribute(string columnNames, string comparisonColumns, string comparisonValues)
        {
            try
            {
                var tableName = typeof(TEntity).Name;
                string whereCondition = string.Empty;
                var compCols = comparisonColumns.Split(",");
                var compColsValue = comparisonValues.Split(",");
                for (int i = 0; i < compCols.Length; i++)
                {
                    if (i == compCols.Length - 1)
                        whereCondition += (compCols[i] + " = @" + compCols[i]);
                    else
                        whereCondition += (compCols[i] + " = @" + compCols[i] + " and ");
                }

                string query = $"SELECT {columnNames} FROM [{tableName}] WHERE {whereCondition}";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        for (int ind = 0; ind < compCols.Length; ind++)
                        {
                            cmd.Parameters.AddWithValue("@" + compCols[ind], compColsValue[ind]);
                        }

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            if (!dr.HasRows)
                            {
                                throw new Exception("No Data Found");
                            }

                            TEntity entity = Activator.CreateInstance<TEntity>();
                            var list = columnNames.Split(",");
                            while (await dr.ReadAsync())
                            {
                                foreach (var property in typeof(TEntity).GetProperties())
                                {
                                    if (list.Contains(property.Name) && dr[property.Name] != DBNull.Value)
                                    {
                                        property.SetValue(entity, dr[property.Name]);
                                    }
                                }
                            }

                            return entity;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Trace: Exception type: {ex.GetType()}, Message: {ex.Message}");
                Debug.WriteLine($"Debug: Exception type: {ex.GetType()}, Message: {ex.Message}");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<TEntity> Find(string columnNames = "", string compParameter = "Id", string compValue = "")
        {


            try
            {
                string tableName = typeof(TEntity).Name;
                string query;
                if (columnNames == "")
                    query = $"SELECT * FROM [{tableName}] where {compParameter} = @compValue";
                else
                    query = $"SELECT {columnNames} FROM [{tableName}] where {compParameter} = @compValue";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return await connection.QuerySingleOrDefaultAsync<TEntity>(query, new { compValue = compValue });
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Trace: Exception type: {ex.GetType}, Message: {ex.Message}");
                Debug.WriteLine($"Debug: Exception type: {ex.GetType}, Message: {ex.Message}");
                throw new Exception("No Data Found", ex);
            }
        }

        public async Task Update(TEntity entity)
        {
            try
            {
                string tableName = typeof(TEntity).Name;
                var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id");

                var values = string.Join(",", properties.Select(y => y.Name + " = @" + y.Name));

                string query = $"update {tableName} set {values} where Id= @Id";
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Create a new DynamicParameters object to handle parameters
                    var parameters = new DynamicParameters();
                    foreach (var property in properties)
                    {
                        parameters.Add($"@{property.Name}", property.GetValue(entity));
                    }

                    var idProperty = typeof(TEntity).GetProperty("Id");
                    if (idProperty != null)
                        parameters.Add("@Id", idProperty.GetValue(entity));

                    // Execute the update query asynchronously
                    await connection.ExecuteAsync(query, parameters);
                }


            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Trace: Exception type: {ex.GetType}, Message: {ex.Message}");
                Debug.WriteLine($"Debug: Exception type: {ex.GetType}, Message: {ex.Message}");
                throw new Exception("No Data Found", ex);
            }
        }

        public async Task Update(string columnNames, TEntity entity)
        {
            try
            {
                var tableName = typeof(TEntity).Name;
                var list = columnNames.Split(',');
                var parameterNames = string.Empty;

                for (int i = 0; i < list.Length; i++)
                {
                    if (i == list.Length - 1)
                        parameterNames += $"{list[i]} = @{list[i]}";
                    else
                        parameterNames += $"{list[i]} = @{list[i]}, ";
                }

                string query = $"UPDATE [{tableName}] SET {parameterNames} WHERE Id = @Id";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    foreach (var property in typeof(TEntity).GetProperties())
                    {
                        if (list.Contains(property.Name))
                        {
                            parameters.Add($"@{property.Name}", property.GetValue(entity));
                        }
                    }

                    // Assuming 'Id' is a property in your entity
                    var idProperty = typeof(TEntity).GetProperty("Id");
                    if (idProperty != null)
                    {
                        parameters.Add("@Id", idProperty.GetValue(entity));
                    }
                    else
                    {
                        throw new InvalidOperationException("Id property is missing in the entity.");
                    }

                    await connection.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Trace: Exception type: {ex.GetType()}, Message: {ex.Message}");
                Debug.WriteLine($"Debug: Exception type: {ex.GetType()}, Message: {ex.Message}");
                throw new Exception("An error occurred while updating the entity", ex);
            }
        }


    }
}


