using System;

namespace buyyu.DDD
{
	[Serializable]
	public class AggregateRootInvalidStateException : Exception
	{
		public AggregateRootInvalidStateException()
		{
		}

		public AggregateRootInvalidStateException(string message) : base(message)
		{
		}

		public AggregateRootInvalidStateException(string message, Exception inner) : base(message, inner)
		{
		}

		protected AggregateRootInvalidStateException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}