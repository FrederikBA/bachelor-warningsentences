using Ardalis.Specification.EntityFrameworkCore;
using Shared.Integration.Interfaces;
using WS.Core.Interfaces.Repositories;

namespace WS.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
{
    public readonly WarningSentenceContext WarningSentenceContext;
    
    public EfRepository(WarningSentenceContext dbContext) : base(dbContext)
    {
        WarningSentenceContext = dbContext;
    }
}