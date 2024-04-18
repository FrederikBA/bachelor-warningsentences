using Ardalis.Specification;

namespace WS.Core.Interfaces.Repositories;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
}