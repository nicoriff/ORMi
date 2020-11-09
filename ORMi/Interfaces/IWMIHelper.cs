using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Interfaces
{
    public interface IWMIHelper
    {
        /// <summary>
        /// Adds a new WMI Instance
        /// </summary>
        /// <param name="obj">Object to add. The classname and properties or corresponding attributes will be maped to the corresponding WMI structure</param>
        void AddInstance(object obj);

        /// <summary>
        /// Adds a new WMI Instance asynchronously
        /// </summary>
        /// <param name="obj">Object to add. The classname and properties or corresponding attributes will be maped to the corresponding WMI structure</param>
        /// <returns></returns>
        Task AddInstanceAsync(object obj);

        /// <summary>
        /// Runs a WMI bulk insertion. If there are any errors on the bulk insert, it will throw an AggregateException at the end of the run. You might want to catch that exception.
        /// </summary>
        /// <param name="instances">List of objects containing all the instances to insert</param>
        void BulkInsert(List<object> instances);

        /// <summary>
        /// Runs an asynchronous WMI bulk insertion. If there are any errors on the bulk insert, it will throw an AggregateException at the end of the run. You might want to catch that exception.
        /// </summary>
        /// <param name="instances">List of objects containing all the instances to insert</param>
        /// <returns></returns>
        Task BulkInsertAsync(List<object> instances);

        /// <summary>
        /// Runs a WMI bulk update. If there are any errors on the bulk update, it will throw an AggregateException at the end of the run. You might want to catch that exception.
        /// </summary>
        /// <param name="instances">List of objects containing all the instances to update</param>
        /// <returns></returns>
        void BulkUpdate(List<object> instances);

        /// <summary>
        /// Runs an asynchronous WMI bulk update. If there are any errors on the bulk update, it will throw an AggregateException at the end of the run. You might want to catch that exception.
        /// </summary>
        /// <param name="instances">List of objects containing all the instances to update</param>
        /// <returns></returns>
        Task BulkUpdateAsync(List<object> instances);

        /// <summary>
        /// Runs a query against WMI. It will return a IEnumerable of dynamic type. No type mapping is done.
        /// </summary>
        /// <param name="query">Query to be run against WMI</param>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string query);

        /// <summary>
        /// Runs a query against WMI. It will return all instances of the class corresponding to the WMI class set on the Type on IEnumerable.
        /// </summary>
        /// <typeparam name="T">The Type of IEnumerable that will be returned</typeparam>
        /// <returns></returns>
        IEnumerable<T> Query<T>();

        /// <summary>
        /// Runs a custom query against WMI.
        /// </summary>
        /// <typeparam name="T">The Type of IEnumerable that will be returned</typeparam>
        /// <param name="query">Query to be run against WMI</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string query);

        /// <summary>
        /// Runs a async query against WMI. It will return a IEnumerable of dynamic type. No type mapping is done.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryAsync(string query);

        /// <summary>
        /// Runs an async query against WMI.
        /// </summary>
        /// <typeparam name="T">The Type of IEnumerable that will be returned</typeparam>
        /// <param name="query">Query to be run against WMI</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>();

        /// <summary>
        /// Runs an async query against WMI. It will return all instances of the class corresponding to the WMI class set on the Type on IEnumerable.
        /// </summary>
        /// <typeparam name="T">The Type of IEnumerable that will be returned</typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string query);

        /// <summary>
        /// Runs a custom query against WMI returning a single value.
        /// </summary>
        /// <param name="query">Query to be run</param>
        /// <returns></returns>
        dynamic QueryFirstOrDefault(string query);

        /// <summary>
        /// Runs a query against WMI. It will return the first instance of the specified Type.
        /// </summary>
        /// <typeparam name="T">The Type of object that will be returned</typeparam>
        /// <returns></returns>
        T QueryFirstOrDefault<T>();

        /// <summary>
        /// Runs an custom query against WMI returning a single value of specified Type.
        /// </summary>
        /// <typeparam name="T">The Type of object that will be returned</typeparam>
        /// <param name="query">Query to be run</param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(string query);

        /// <summary>
        /// Runs an async query against WMI returning a single value.
        /// </summary>
        /// <param name="query">Query to be run</param>
        /// <returns></returns>
        Task<dynamic> QueryFirstOrDefaultAsync(string query);

        /// <summary>
        /// Runs an async query against WMI. It will return the first instance of the specified Type.
        /// </summary>
        /// <typeparam name="T">The Type of object that will be returned</typeparam>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>();

        /// <summary>
        /// Runs an async query against WMI returning a single value.
        /// </summary>
        /// <param name="query">Query to be run</param>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string query);

        /// <summary>
        /// Remove a WMI instance.
        /// </summary>
        /// <param name="obj">Object to be removed.</param>
        void RemoveInstance(object obj);

        /// <summary>
        /// Remove WMI instances based on a custom query.
        /// </summary>
        /// <param name="query">Query that returns the objects to be removed</param>
        void RemoveInstance(string query);

        /// <summary>
        /// Remove a WMI instance asynchronously.
        /// </summary>
        /// <param name="obj">Object to be removed.</param>
        /// <returns></returns>
        Task RemoveInstanceAsync(object obj);

        /// <summary>
        /// Remove WMI instances based on a custom query asynchronously.
        /// </summary>
        /// <param name="query">Query that returns the objects to be removed</param>
        /// <returns></returns>
        Task RemoveInstanceAsync(string query);

        /// <summary>
        /// Modifies an existing instance.
        /// </summary>
        /// <param name="obj">Object to be updated. ORMi will search the property with the SearchKey attribute. That value is going to be used to make the update.</param>
        void UpdateInstance(object obj);

        /// <summary>
        /// Modifies an existing instance based on a custom query.
        /// </summary>
        /// <param name="obj">Object to be updated</param>
        /// <param name="query">Query to be run against WMI. The resulting instances will be updated</param>
        void UpdateInstance(object obj, string query);

        /// <summary>
        /// Modifies an existing instance asynchonously.
        /// </summary>
        /// <param name="obj">Object to be updated. ORMi will search the property with the SearchKey attribute. That value is going to be used to make the update.</param>
        /// <returns></returns>
        Task UpdateInstanceAsync(object obj);

        /// <summary>
        /// Modifies an existing instance based on a custom query asynchonously.
        /// </summary>
        /// <param name="obj">Object to be updated</param>
        /// <param name="query">Query to be run. The resulting instances will be updated</param>
        /// <returns></returns>
        Task UpdateInstanceAsync(object obj, string query);
    }
}
