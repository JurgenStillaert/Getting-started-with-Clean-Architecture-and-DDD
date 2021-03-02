namespace buyyu.DDD
{
	public abstract class Entity<TKey> where TKey : Value<TKey>
	{
		public TKey Id { get; protected set; }
	}
}