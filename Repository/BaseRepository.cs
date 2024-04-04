using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepository<T> : IRepository<T>
    {
        public ISqlConnectionInformation ConnectionInformation { get; set; }
        public virtual Task<List<T>> Get(int Id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<List<T>> GetAll(string code)
        {
            throw new NotImplementedException();
        }
        public virtual Task<T> Get(string code)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> Get()
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get(bool status)
        {
            throw new NotImplementedException();
        }

        public virtual T Load<T>(IDataReader dataReader) where T : new()
        {
            var model = new T();
            var properties = typeof(T).GetProperties().ToList();
            var dbColumns = Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToList();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if (dbColumns.Contains(propertyName))
                {
                    var propertyType = property.PropertyType.Name;
                    if (propertyType.Contains("Nullable"))
                    {
                        propertyType = property.PropertyType.GenericTypeArguments[0].Name;
                    }

                    var entityObject = RuntimeHelpers.GetObjectValue(model);
                    if (dataReader[propertyName] == DBNull.Value)
                    {
                        model.GetType().GetProperty(property.Name)?.SetValue(entityObject, null);
                    }
                    else
                    {
                        switch (propertyType)
                        {
                            case "Int32":
                                model.GetType().GetProperty(property.Name)?.SetValue(entityObject, Convert.ToInt32(dataReader[propertyName]));
                                break;
                            case "Int64":
                                model.GetType().GetProperty(property.Name)?.SetValue(entityObject, Convert.ToInt64(dataReader[propertyName]));
                                break;
                            case "String":
                            case "string":
                                model.GetType().GetProperty(property.Name)?.SetValue(entityObject, Convert.ToString(dataReader[propertyName]));
                                break;
                            case "Boolean":
                                model.GetType().GetProperty(property.Name)?.SetValue(entityObject, Convert.ToBoolean(dataReader[propertyName]));
                                break;
                            case "DateTime":
                                model.GetType().GetProperty(property.Name)?.SetValue(entityObject, Convert.ToDateTime(dataReader[propertyName]));
                                break;
                        }
                    }
                }
            }
            return model;
        }

        public virtual Task<T> Post(T model)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Put(T model)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Delete(T model)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get(T model)
        {
            throw new NotImplementedException();
        }

        public virtual Task<UserDetailResponse> AddUser(T model)
        {
            throw new NotImplementedException();
        }

        public virtual Task<UserDetailResponse> UpdateUser(T model)
        {
            throw new NotImplementedException();
        }
    }
}
