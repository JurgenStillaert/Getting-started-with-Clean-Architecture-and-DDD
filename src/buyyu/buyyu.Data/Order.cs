using System;
using System.Collections.Generic;
using System.Linq;

namespace buyyu.Data
{
	public class Order
	{
		public Guid Id { get; private set; }
		public Guid ClientId { get; private set; }
		public Guid OrderStateId { get; private set; }
		public DateTime OrderDate { get; private set; }
		public decimal TotalAmount { get; private set; }
		public decimal PaidAmount { get; private set; }

		public OrderState State { get; private set; }
		public List<Orderline> Lines { get; private set; }
		public List<Payment> Payments { get; private set; }

		public static Order Create(
			Guid clientId,
			Guid orderStateId)
		{
			if (clientId == null || clientId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(clientId), "ClientId cannot be empty");
			}
			if (orderStateId == null || orderStateId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(orderStateId), "OrderStateId cannot be empty");
			}

			var order = new Order
			{
				ClientId = clientId,
				OrderStateId = orderStateId,
				OrderDate = DateTime.Now,
				PaidAmount = 0,
				Payments = new List<Payment>(),
				Lines = new List<Orderline>()
			};

			return order;
		}

		public void AddOrderline(Guid productId, decimal price, int qty)
		{
			if (productId == null || productId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(productId), "ProductId cannot be empty");
			}

			if (Lines.Any(ol => ol.ProductId == productId))
			{
				throw new InvalidOperationException("Product is already added");
			}

			if (qty <= 0)
			{
				throw new ArgumentNullException(nameof(qty), "Qty must be a positive integer");
			}

			Lines.Add(Orderline.Create(productId, price, qty));

			TotalAmount = Lines.Select(x => x.Price * x.Qty).Sum();
		}

		public void UpdateOrderline(Guid productId, decimal price, int qty)
		{
			if (productId == null || productId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(productId), "ProductId cannot be empty");
			}

			if (!Lines.Any(ol => ol.ProductId == productId))
			{
				throw new InvalidOperationException("Product is not found");
			}

			if (qty <= 0)
			{
				throw new ArgumentNullException(nameof(qty), "Qty must be a positive integer");
			}

			var orderline = Lines.First(ol => ol.ProductId == productId);

			orderline.Update(price, qty);

			TotalAmount = Lines.Select(x => x.Price * x.Qty).Sum();
		}

		public void RemoveOrderline(Guid productId)
		{
			Lines.Remove(Lines.First(ol => ol.ProductId == productId));

			TotalAmount = Lines.Select(x => x.Price * x.Qty).Sum();
		}

		public void Confirm(Guid confirmState)
		{
			OrderStateId = confirmState;
			OrderDate = DateTime.Now;
		}

		public void Ship(Guid shipState)
		{
			OrderStateId = shipState;
			OrderDate = DateTime.Now;
		}

		public void ReceivePayment(decimal amount)
		{
			PaidAmount += amount;

			Payments.Add(Payment.Create(amount));
		}
	}
}