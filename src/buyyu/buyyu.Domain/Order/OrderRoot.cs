using buyyu.DDD;
using buyyu.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using static buyyu.Models.Events.OrderEvents;

namespace buyyu.Domain.Order
{
	public class OrderRoot : AggregateRoot<OrderId>
	{
		public ClientId ClientId { get; private set; }
		public OrderDate OrderDate { get; private set; }
		public Money TotalAmount { get; private set; }
		public Money PaidAmount { get; private set; }
		public OrderState State { get; private set; }
		public List<Orderline> Lines { get; private set; }

		public static OrderRoot Create(OrderId orderId, ClientId clientId)
		{
			var order = new OrderRoot();

			order.Apply(new v1.OrderCreated(orderId, clientId, DateTime.Now));

			return order;
		}

		public void AddOrderline(ProductId productId, Money price, Quantity qty)
		{
			if (!State.IsNewState())
			{
				throw new InvalidOperationException("Cannot add orderlines to a confirmed order");
			}

			if (Lines.Any(ol => ol.ProductId == productId))
			{
				throw new InvalidOperationException("Product is already added");
			}

			Apply(new v1.OrderlineAdded(Id, OrderlineId.GenerateNew(), productId, price.Amount, price.Currency, qty));
		}

		public void UpdateOrderline(ProductId productId, Money price, Quantity qty)
		{
			if (!State.IsNewState())
			{
				throw new InvalidOperationException("Cannot update orderlines to a confirmed order");
			}

			if (!Lines.Any(ol => ol.ProductId == productId))
			{
				throw new InvalidOperationException("Product is not found");
			}

			Apply(new v1.OrderlineUpdated(Id, productId, price.Amount, price.Currency, qty));
		}

		public void RemoveOrderline(ProductId productId)
		{
			if (!State.IsNewState())
			{
				throw new InvalidOperationException("Cannot remove orderlines to a confirmed order");
			}

			Apply(new v1.OrderlineRemoved(Id, productId));
		}

		public void Confirm()
		{
			if (!State.IsNewState())
			{
				throw new InvalidOperationException("Cannot confirm order that already has been confirmed");
			}

			if (Lines.DefaultIfEmpty().Sum(ol => ol.Qty) == 0)
			{
				throw new Exception("Order does not have any products and cannot be confirmed");
			}

			Apply(new v1.OrderConfirmed(Id, OrderDate.Now()));
		}

		public void MarkShipped()
		{
			if (!State.IsConfirmedState())
			{
				throw new InvalidOperationException("Cannot confirm not confirmed order");
			}

			Apply(new v1.MarkedShipped(Id));
		}

		public void MarkPaid(Money amount)
		{
			if (State.IsNewState())
			{
				throw new InvalidOperationException("Cannot pay not confirmed order");
			}

			Apply(new v1.MarkedPaid(Id, amount.Amount, amount.Currency));
		}

		protected override void EnsureValidState()
		{
			var isValid = true;

			if (Id == null) isValid = false;
			if (ClientId == null) isValid = false;
			if (OrderDate == null) isValid = false;
			if (PaidAmount == null) isValid = false;
			if (Lines == null) isValid = false;

			if (!State.IsNewState())
			{
				if (Lines.Count == 0) isValid = false;
			}

			if (!isValid)
			{
				throw new AggregateRootInvalidStateException();
			}
		}

		#region Handlers

		private void Handle(v1.OrderCreated @event)
		{
			Id = OrderId.FromGuid(@event.OrderId);
			ClientId = ClientId.FromGuid(@event.ClientId);
			State = OrderState.FromEnum(OrderState.OrderStateEnum.NEW);
			OrderDate = OrderDate.FromDateTime(@event.OrderDate);
			TotalAmount = Money.Empty("EUR");
			PaidAmount = Money.Empty("EUR");
			Lines = new List<Orderline>();
		}

		private void Handle(v1.OrderlineAdded @event)
		{
			Lines.Add(Orderline.Create(
				OrderlineId.FromGuid(@event.OrderlineId),
				ProductId.FromGuid(@event.ProductId),
				Money.FromDecimalAndCurrency(@event.Price, @event.Currency),
				Quantity.FromInt(@event.Quantity)));

			TotalAmount = Money.FromDecimalAndCurrency(Lines.Select(x => x.Price.Amount * x.Qty).Sum(), "EUR");
		}

		private void Handle(v1.OrderlineUpdated @event)
		{
			var orderline = Lines.First(ol => ol.ProductId == @event.ProductId);

			orderline.Update(Money.FromDecimalAndCurrency(@event.Price, @event.Currency), Quantity.FromInt(@event.Quantity));

			TotalAmount = Money.FromDecimalAndCurrency(Lines.Select(x => x.Price.Amount * x.Qty).Sum(), "EUR");
		}

		private void Handle(v1.OrderlineRemoved @event)
		{
			Lines.Remove(Lines.First(ol => ol.ProductId == @event.ProductId));

			TotalAmount = Money.FromDecimalAndCurrency(Lines.Select(x => x.Price.Amount * x.Qty).Sum(), "EUR");
		}

		private void Handle(v1.OrderConfirmed @event)
		{
			State = OrderState.FromEnum(OrderState.OrderStateEnum.CNF);
			OrderDate = OrderDate.FromDateTime(@event.OrderDate);
		}

		private void Handle(v1.MarkedShipped @event)
		{
			State = OrderState.FromEnum(OrderState.OrderStateEnum.SHP);
		}

		private void Handle(v1.MarkedPaid @event)
		{
			PaidAmount += Money.FromDecimalAndCurrency(@event.Amount, @event.Currency);
		}

		#endregion Handlers
	}
}