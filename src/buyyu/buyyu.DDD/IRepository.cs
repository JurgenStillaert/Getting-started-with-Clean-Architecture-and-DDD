using System.Threading.Tasks;

namespace buyyu.DDD
{
	public interface IRepository<TAggregate, TKey>
		 where TAggregate : AggregateRoot<TKey>
		where TKey : Value<TKey>
	{
		Task Save(TAggregate aggregateRoot);

		Task<TAggregate> Load(TKey aggregateId);

		Task Delete(TAggregate aggregateRoot);

		Task Add(TAggregate aggregateRoot);
	}
}