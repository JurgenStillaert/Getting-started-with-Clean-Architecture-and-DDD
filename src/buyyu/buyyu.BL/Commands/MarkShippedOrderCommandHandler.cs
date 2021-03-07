using buyyu.BL.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class MarkShippedOrderCommandHandler : UpdateCommandHandler<MarkShippedOrderCommand, OrderRoot, OrderId>
	{
		private readonly IMailService _mailService;

		public MarkShippedOrderCommandHandler(
			IRepository<OrderRoot, OrderId> repo,
			IMailService mailService)
			: base(repo)
		{
			_mailService = mailService;
		}

		protected override void PreHandle(MarkShippedOrderCommand command)
		{
			AggregateId = OrderId.FromGuid(command.OrderId);
		}

		protected async override Task Apply(MarkShippedOrderCommand command)
		{
			AggregateRoot.MarkShipped();

			//Send email
			await _mailService.SendOrderShippedMail(AggregateRoot);
		}
	}
}