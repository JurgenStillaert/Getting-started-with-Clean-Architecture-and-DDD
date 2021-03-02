using buyyu.DDD;
using System;

namespace buyyu.Domain.Order
{
	public class ClientId : Value<ClientId>
	{
		public Guid Value { get; private set; }

		//Satisfy EF Core
		private ClientId() {}

		private ClientId(Guid clientId)
		{
			if (clientId == null || clientId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(clientId), "ClientId cannot be empty");
			}

			Value = clientId;
		}

		public static ClientId FromGuid(Guid clientId) => new ClientId(clientId);
		public static ClientId FromString(string clientId) => new ClientId(Guid.Parse(clientId));

		public static implicit operator Guid(ClientId clientId) => clientId.Value;
	}
}