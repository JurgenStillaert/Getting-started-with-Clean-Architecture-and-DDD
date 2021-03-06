using buyyu.DDD;
using buyyu.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

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
			var order = new OrderRoot
			{
				Id = orderId,
				ClientId = clientId,
				State = OrderState.FromEnum(OrderState.OrderStateEnum.NEW),
				OrderDate = OrderDate.Now(),
				TotalAmount = Money.Empty("EUR"),
				PaidAmount = Money.Empty("EUR"),
				Lines = new List<Orderline>()
			};

			order.EnsureValidation();

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

			Lines.Add(Orderline.Create(OrderlineId.GenerateNew(), productId, price, qty));

			TotalAmount = Money.FromDecimalAndCurrency(Lines.Select(x => x.Price.Amount * x.Qty).Sum(), "EUR");

			EnsureValidation();
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

			var orderline = Lines.First(ol => ol.ProductId == productId);

			orderline.Update(price, qty);

			TotalAmount = Money.FromDecimalAndCurrency(Lines.Select(x => x.Price.Amount * x.Qty).Sum(), "EUR");

			EnsureValidation();
		}

		public void RemoveOrderline(ProductId productId)
		{
			if (!State.IsNewState())
			{
				throw new InvalidOperationException("Cannot remove orderlines to a confirmed order");
			}

			Lines.Remove(Lines.First(ol => ol.ProductId == productId));

			TotalAmount = Money.FromDecimalAndCurrency(Lines.Select(x => x.Price.Amount * x.Qty).Sum(), "EUR");

			EnsureValidation();
		}

		public void Confirm()
		{
			if (!State.IsNewState())
			{
				throw new InvalidOperationException("Cannot confirm order that already has been confirmed");
			}

			State = OrderState.FromEnum(OrderState.OrderStateEnum.CNF);
			OrderDate = OrderDate.Now();

			EnsureValidation();
		}

		public void MarkShipped()
		{
			if (!State.IsConfirmedState())
			{
				throw new InvalidOperationException("Cannot confirm not confirmed order");
			}

			State = OrderState.FromEnum(OrderState.OrderStateEnum.SHP);
			OrderDate = OrderDate.Now();

			EnsureValidation();
		}

		public void MarkPaid(Money amount)
		{
			if (State.IsNewState())
			{
				throw new InvalidOperationException("Cannot pay not confirmed order");
			}

			PaidAmount += amount;

			EnsureValidation();
		}

		protected override void EnsureValidation()
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
	}
}