using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleDocumentStore
{
    public interface ISimpleDocumentStore<T> where T : IComparable
    {
        /// <summary>
        /// Creates a new SimpleDocument
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task InsertAsync(SimpleDocument<T> document);

        /// <summary>
        /// Updates a new SimpleDocument
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task UpdateAsync(SimpleDocument<T> document);


        /// <summary>
        /// Delete the SimpleDocument
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        Task DeleteAsync(string id);

        /// <summary>
        /// Finds the SimpleDocument
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SimpleDocument<T>> FetchAsync(string id);

        /// <summary>
        /// Pages all SimpleDocuments
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pagingState"></param>
        /// <returns></returns>
        Task<IPage<SimpleDocument<T>>> PageAsync(int pageSize,
            byte[] pagingState);


       
    }
}
