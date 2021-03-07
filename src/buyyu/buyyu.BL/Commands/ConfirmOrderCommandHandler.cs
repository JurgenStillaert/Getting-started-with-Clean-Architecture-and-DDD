using buyyu.BL.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class ConfirmOrderCommandHandler : UpdateCommandHandler<ConfirmOrderCommand, OrderRoot, OrderId>
	{
		private readonly IMailService _mailService;

		public ConfirmOrderCommandHandler(
			IRepository<OrderRoot, OrderId> repo,
			IMailService mailService)
			: base(repo)
		{
			_mailService = mailService;
		}

		protected override void PreHandle(ConfirmOrderCommand command)
		{
			AggregateId = OrderId.FromGuid(command.OrderId);
		}

		protected async override Task Apply(ConfirmOrderCommand command)
		{
			AggregateRoot.Confirm();

			//Send email
			await _mailService.SendOrderConfirmationMail(AggregateRoot);
		}
	}
}