using System.Threading;
using System.Threading.Tasks;

namespace LBH.Data.Repository
{
    public interface IRepository<T> //where T //: IEntity
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        //Task<T> ReadAsync(TIndex index, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
        //Task<bool> DeleteAsync(TIndex index, CancellationToken cancellationToken);
    }
}
