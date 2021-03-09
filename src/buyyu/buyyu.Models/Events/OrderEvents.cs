using buyyu.DDD;
using System;

namespace buyyu.Models.Events
{
	public static class OrderEvents
	{
		public static class v1
		{
			public sealed class OrderCreated : IDomainEvent
			{
				public OrderCreated(
					Guid orderId,
					Guid clientId,
					DateTime orderDate)
				{
					OrderId = orderId;
					ClientId = clientId;
					OrderDate = orderDate;
				}

				public Guid OrderId { get; }
				public Guid ClientId { get; }
				public DateTime OrderDate { get; }
			}

			public sealed class OrderlineAdded : IDomainEvent
			{
				public OrderlineAdded(
					Guid orderId,
					Guid orderlineId,
					Guid productId,
					decimal price,
					string currency,
					int quantity)
				{
					OrderId = orderId;
					OrderlineId = orderlineId;
					ProductId = productId;
					Price = price;
					Currency = currency;
					Quantity = quantity;
				}

				public Guid OrderId { get; }
				public Guid OrderlineId { get; }
				public Guid ProductId { get; }
				public decimal Price { get; }
				public string Currency { get; }
				public int Quantity { get; }
			}

			public sealed class OrderlineUpdated : IDomainEvent
			{
				public OrderlineUpdated(
					Guid orderId,
					Guid productId,
					decimal price,
					string currency,
					int quantity)
				{
					OrderId = orderId;
					ProductId = productId;
					Price = price;
					Currency = currency;
					Quantity = quantity;
				}

				public Guid OrderId { get; }
				public Guid ProductId { get; }
				public decimal Price { get; }
				public string Currency { get; }
				public int Quantity { get; }
			}

			public sealed class OrderlineRemoved : IDomainEvent
			{
				public OrderlineRemoved(Guid orderId, Guid productId)
				{
					OrderId = orderId;
					ProductId = productId;
				}

				public Guid OrderId { get; }
				public Guid ProductId { get; }
			}

			public sealed class OrderConfirmed : IDomainEvent
			{
				public OrderConfirmed(Guid orderId, DateTime orderDate)
				{
					OrderId = orderId;
					OrderDate = orderDate;
				}

				public Guid OrderId { get; }
				public DateTime OrderDate { get; }
			}

			public sealed class MarkedShipped : IDomainEvent
			{
				public MarkedShipped(Guid orderId)
				{
					OrderId = orderId;
				}

				public Guid OrderId { get; }
			}

			public sealed class MarkedPaid : IDomainEvent
			{
				public MarkedPaid(Guid orderId, decimal amount, string currency)
				{
					OrderId = orderId;
					Amount = amount;
					Currency = currency;
				}

				public Guid OrderId { get; }
				public decimal Amount { get; }
				public string Currency { get; }
			}
		}
	}
}