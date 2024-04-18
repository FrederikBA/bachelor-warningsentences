using Ardalis.Specification;
using Shared.Integration.Interfaces;

namespace WS.Core.Interfaces.Repositories;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}