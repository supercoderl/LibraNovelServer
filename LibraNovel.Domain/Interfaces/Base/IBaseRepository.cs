using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Domain.Interfaces.Base
{
    public interface IBaseRepository<T> where T : class
    {
        /*
            This is where we put all the methods
            that are common for all entities.
         */

        IReadOnlyList<T> GetAll();
        T GetById(Guid id);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
