using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repo
{
    public interface IRepo<T>  where T : class
    { 

        Task<T> SaveAsync(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetById(Guid id);
    }
}
