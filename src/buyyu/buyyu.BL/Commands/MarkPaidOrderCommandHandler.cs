using buyyu.BL.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class MarkPaidOrderCommandHandler : UpdateCommandHandler<MarkPaidOrderCommand, OrderRoot, OrderId>
	{
		private readonly IMailService _mailService;

		public MarkPaidOrderCommandHandler(
			IRepository<OrderRoot, OrderId> repo,
			IMailService mailService)
			: base(repo)
		{
			_mailService = mailService;
		}

		protected override void PreHandle(MarkPaidOrderCommand command)
		{
			AggregateId = OrderId.FromGuid(command.OrderId);
		}

		protected async override Task Apply(MarkPaidOrderCommand command)
		{
			AggregateRoot.MarkPaid(Money.FromDecimalAndCurrency(command.Amount, "EUR"));

			//Send email
			await _mailService.SendPaymentConfirmationMail(AggregateRoot);
		}
	}
}