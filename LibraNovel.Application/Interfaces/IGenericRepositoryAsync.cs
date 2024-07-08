using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IGenericRepositoryAsync<T, Y, U> 
        where T : class
        where Y : class
        where U : class
    {
        Task<Response<T>> GetByIdAsync(int id);
        Task<Response<IReadOnlyList<T>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Response<T>> AddAsync(Y request);
        Task<Response<string>> UpdateAsync(int id, U request);
        Task<Response<string>> DeleteAsync(int id);
    }
}
