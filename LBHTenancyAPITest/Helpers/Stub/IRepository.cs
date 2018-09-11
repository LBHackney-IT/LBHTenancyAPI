using System.Threading;
using System.Threading.Tasks;

namespace LBHTenancyAPITest.Helpers.Stub
{
    public interface IRepository<T> 
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
    }
}
