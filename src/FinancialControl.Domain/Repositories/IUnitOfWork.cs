using ProductCatalogue.Domain.BaseTypes;

namespace ProductCatalogue.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public void Commit(IAggregateRoot aggregate);
    }
}
