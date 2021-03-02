using System;

namespace buyyu.DDD
{
	public abstract class AggregateRoot<TKey> where TKey : Value<TKey>
	{
		public TKey Id { get; protected set; }

		protected abstract void EnsureValidation();
	}
}