

namespace Domain.UoW
{
    public interface IDomainUnitOfWork
    {
        TDomain GetDomain<TDomain>() where TDomain : class;
    }
}