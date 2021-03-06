namespace buyyu.DDD
{
	public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : Value<TKey>
	{
		protected abstract void EnsureValidState();
	}
}