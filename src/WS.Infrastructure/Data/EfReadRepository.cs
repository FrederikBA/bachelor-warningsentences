using Ardalis.Specification.EntityFrameworkCore;
using WS.Core.Interfaces.Repositories;

namespace WS.Infrastructure.Data;

public class EfReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public readonly WarningSentenceContext WarningSentenceContext;
    
    public EfReadRepository(WarningSentenceContext dbContext) : base(dbContext)
    {
        WarningSentenceContext = dbContext;
    }
}