using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDocumentStore
{
    public class InMemoryStoreBase<T> where T : class, IDocumentBase, new()
    {
        public delegate bool ContainsAnyInList<T>(List<T> a, List<T> b);

        private ContainsAnyInList<string> _containsAnyInList;

        public ContainsAnyInList<string> DelegateContainsAnyInStringList
        {
            get
            {
                if (_containsAnyInList == null)
                {
                    _containsAnyInList = (a, b) =>
                    {
                        var result = a.Any(x => b.Contains(x));
                        return result;
                    };
                }
                return _containsAnyInList;
            }
        }
        Dictionary<string,T> _collection;
        Dictionary<string, T> Collection
        {
            get
            {
                if (_collection == null)
                {
                    _collection = new Dictionary<string, T>();
                }
                return _collection;
            }
        }
         
        protected static object TheLock
        {
            get { return ConcurrencyLock.TheLock; }
        }


        protected async Task GoAsync(Action action)
        {
            await Task.Run(action);
        }

        protected static async Task<TResult> GoAsync<TResult>(Func<TResult> func)
        {

            var task = Task.Run(func);
            var result = await task;
            return result;

        }

        public async Task InsertAsync(T doc)
        {
            await GoAsync(() =>
            {
                lock (TheLock)
                {
                    Collection[doc.Id] = doc;
                }
            });
        }

        public async Task<T> FetchAsync(string id)
        {
            var result = await GoAsync(() =>
            {
                T value = null;
                lock (TheLock)
                {
                    Collection.TryGetValue(id, out value);
                }
                return value;
            }
            );
            return result;
        }

        public async Task UpdateAsync(T doc)
        {
            await InsertAsync(doc);
        }

        public async Task DeleteAsync(string id)
        {
            await GoAsync(() =>
            {
                lock (TheLock)
                {
                    if (Collection.ContainsKey(id))
                    {
                        Collection.Remove(id);
                    }
                }
            });
        }

        private async Task<List<T>> RetrieveAllAsync()
        {

            var result = await GoAsync(() =>
            {
                List<T> r2 = null;
                lock (TheLock)
                {
                    r2 = (from item in Collection
                                select item.Value).ToList();
                    
                }
                return r2 ?? new List<T>();
            }
            );
            return result;
        }
      
        public async Task<IPage<T>> PageAsync(
            int pageSize,
            byte[] pagingState)
        {
            byte[] currentPagingState = pagingState;
            PagingState ps = pagingState.DeserializePageState();
            var records = await RetrieveAllAsync();

            var slice = records.Skip(ps.CurrentIndex).Take(pageSize).ToList();
            if (slice.Count < pageSize)
            {
                // we are at the end
                pagingState = null;
            }
            else
            {
                ps.CurrentIndex += pageSize;
                pagingState = ps.Serialize();
            }

            var page = new PageContainer<T>(currentPagingState, pagingState, slice);
            return page;
        }

    }
   
}
