using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace buyyu.DDD
{
	public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : Value<TKey>
	{
		private readonly List<IDomainEvent> _changes = new List<IDomainEvent>();
		public IReadOnlyList<IDomainEvent> Changes => _changes.ToList();

		public void ClearChanges() => _changes.Clear();

		/// <summary>
		/// Find Handle methods in the implementation with parameter of type @event
		/// </summary>
		/// <param name="event"></param>
		protected void When(IDomainEvent @event)
		{
			//Get the handle methods
			var handleMethod = this.GetType().GetMethod(
					"Handle",
					BindingFlags.Instance | BindingFlags.NonPublic,
					Type.DefaultBinder,
					new Type[] { @event.GetType() },
					null);

			if (handleMethod == null)
			{
				throw new MissingMethodException($"Handle method with event { @event.GetType()} is missing");
			}

			try
			{
				handleMethod.Invoke(this, new object[] { @event });
			}
			catch (TargetInvocationException targetInvocationException)
			{
				throw targetInvocationException.InnerException;
			}
		}

		protected void Apply(IDomainEvent @event)
		{
			When(@event);
			EnsureValidState();
			_changes.Add(@event);
		}

		public void Replay(List<IDomainEvent> history)
		{
			foreach (var @event in history)
			{
				When(@event);
			}
		}

		protected abstract void EnsureValidState();
	}
}